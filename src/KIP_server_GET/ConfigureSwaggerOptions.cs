﻿using System;
using System.Diagnostics.CodeAnalysis;
using KIP_server_GET.Constants;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KIP_server_GET
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    [ExcludeFromCodeCoverage]
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this._provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in this._provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = $" {CustomNames.KIP_server_GET}: v{CustomNames.Version}",
                Version = CustomNames.Version,
                Description = " Server \"GET\": providing common information for not auth mode",
                Contact = new OpenApiContact
                {
                    Name = " KIP ",
                    Email = " kip.ntu.khpi@gmail.com",
                },
                TermsOfService = new Uri("http://www.kpi.kharkov.ua/ukr/"),
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}