using KIP_server_GET.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace KIP_server_GET
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;

            /*
            var connection = Configuration.GetConnectionString("PostgresConnection");
            services.AddDbContext<Server_GETContext>(options => options.UseNpgsql(connection));
            */

            var pgConnectionString = this.Configuration.GetConnectionString("PostgresConnection");
            var pgVersionString = this.Configuration.GetConnectionString("PostgresVersion");

            services.AddDbServices(pgConnectionString, pgVersionString);
            services.AddMvcCore()
                    .AddDataAnnotations()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, IWebHostEnvironment env)
        {
            app.UseTokens(this.Configuration["Tokens:EntryToken"]);
            var message = $"{CustomNames.KIP_server_GET} uses Tokens Protection";
            logger.Log(LogLevel.Information, message);

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });

            app.UseResponseCaching();
        }
    }
}
