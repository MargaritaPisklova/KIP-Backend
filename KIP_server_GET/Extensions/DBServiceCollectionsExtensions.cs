﻿using System;
using KIP_server_GET.Interfaces;
using KIP_server_GET.Mocks;
using KIP_server_GET.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains DB related extension methods
    /// </summary>
    public static class DbServiceCollectionsExtensions
    {
        /// <summary>
        /// Adds the database services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="pgVersionString">Postgres version string. Must contain 2-4 numerics separated by '.'</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">services</exception>
        /// <exception cref="System.ArgumentException">Connection string must be not null or empty - connectionString</exception>
        public static IServiceCollection AddDbServices(this IServiceCollection services, string connectionString, string pgVersionString)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Connection string must be not null or empty", nameof(connectionString));
            if (string.IsNullOrEmpty(pgVersionString)) throw new ArgumentException("Postgres version string must be not null or empty", nameof(pgVersionString));

            /*
            var pgVersion = new Version(pgVersionString);
            services.AddDbContextPool<Server_GETContext>(contextOptions =>
            {
                contextOptions.UseNpgsql(connectionString, npgOptions =>
                {
                    npgOptions.MigrationsAssembly("KIP_server_GET.Migrations")
                        .EnableRetryOnFailure();
                    npgOptions.SetPostgresVersion(pgVersion);
                });
            });
            */

            services.AddTransient<IFaculty, MockFaculty>();

            return services;
        }
    }
}
