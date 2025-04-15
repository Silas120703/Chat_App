﻿using Chat_App_Shared.Helpers;
using Chat_App_Shared.Interfaces;
using Chat_App_Shared.Interfaces.Repositories;
using Chat_App_Shared.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Chat_App_Shared.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection RegisterRepositories(this IServiceCollection services, string assemblyName)
		{
			var repoServices = ApplicationHelper.GetApplicationClasses(assemblyName)
			.Where(t => t.IsAssignableTo(typeof(IRepository)))
			.ToList();

			foreach (var repositoryService in repoServices)
			{
				services.AddScoped(repositoryService);
			}
			return services;
		}

		public static IServiceCollection RegisterServices(this IServiceCollection services, string assemblyName)
		{
			var servicesList = ApplicationHelper.GetApplicationClasses(assemblyName)
			.Where(t => t.IsAssignableTo(typeof(IService)))
			.ToList();
			foreach (var service in servicesList)
			{
				services.AddScoped(service);
			}
			return services;
		}

		public static IServiceCollection AddDatabase<DbC>(this IServiceCollection services, string connectionString, string dbMigrationAssembly) where DbC : DbContext
		{
			services.AddDbContextPool<DbC>((provider, options) =>
			{
				options
					.UseMySql(
						connectionString,
						ServerVersion.AutoDetect(connectionString),
						mysqlOptions =>
							mysqlOptions
								.MigrationsAssembly(dbMigrationAssembly)
					)
					.EnableSensitiveDataLogging();
				options.ConfigureWarnings(warnings =>
				{
					warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored);
				}
				);
				options.UseApplicationServiceProvider(provider);
			}

			);
			return services;
		}
	}
}
