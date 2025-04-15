using Chat_App_Database.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Chat_App_API.Service
{
	

	public interface IJwtService
	{
		string GenerateToken(User user);
	}

	public class JwtService : IJwtService
	{
		private readonly string _secretKey;
		private readonly string _issuer;
		private readonly string _audience;
		private readonly int _expiryInMinutes;

		public JwtService(IConfiguration configuration)
		{
			_secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:SecretKey is not configured.");
			_issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:Issuer is not configured.");
			_audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:Audience is not configured.");
			_expiryInMinutes = int.Parse(configuration["Jwt:ExpiryInMinutes"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:ExpiryInMinutes is not configured."));
		}

		public string GenerateToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_secretKey);

			var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim("id", user.Id.ToString()),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role)
		};

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(_expiryInMinutes),
				Issuer = _issuer,
				Audience = _audience,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
