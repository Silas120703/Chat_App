using Chat_App_Shared.Interfaces;
using Chat_App_Core.Services;
using Chat_App_Database;
using Chat_App_Database.Repositories;
using Chat_App_Shared.Extensions;
using Chat_App_Shared.Interfaces.Repositories;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Chat_App_Shared.Helpers;
using Chat_App_API.Hubs;
using Chat_App_API.Service;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");	


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApp", Version = "v1" });

	// 🔥 Cấu hình Swagger để hỗ trợ Authorization bằng Bearer Token
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Nhập token vào ô bên dưới (không cần 'Bearer ')"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowReactApp",
		policy =>
		{
			policy.WithOrigins("http://localhost:3000") // Địa chỉ frontend
				  .AllowAnyHeader()
				  .AllowAnyMethod()
				  .AllowCredentials(); // Nếu dùng cookie, JWT
		});
});

builder.Services.AddDatabase<ChatAppDBContext>(connectionString, nameof(Chat_App_Database));
builder.Services.RegisterRepositories("Chat_App");
builder.Services.RegisterServices("Chat_App");
builder.Services.AddScoped<EmailHelper>();
// Đăng ký dịch vụ JWT
builder.Services.AddSingleton<JwtService>();

// Cấu hình xác thực JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(secretKey),
		ValidateIssuer = true,
		ValidIssuer = jwtSettings["Issuer"],
		ValidateAudience = true,
		ValidAudience = jwtSettings["Audience"],
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero // Tránh lỗi lệch thời gian
	};
	options.Events = new JwtBearerEvents
	{
		OnAuthenticationFailed = context =>
		{
			Console.WriteLine($"Authentication failed: {context.Exception.Message}");
			return Task.CompletedTask;
		},
		OnTokenValidated = context =>
		{
			Console.WriteLine("Token validated successfully!");
			return Task.CompletedTask;
		}
	};
});

builder.Services.AddSignalR();


//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(Repository<>));
//builder.Services.AddScoped<UserService>();
//builder.Services.AddScoped<EmailService>();
//builder.Services.AddScoped(typeof(IGenericService<>), typeof(Service<>));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors("AllowReactApp");

// Sử dụng xác thực và ủy quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

app.Run();
