using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ProductApiExample.Api.Swagger
{
    internal static class Extensions
    {
        /// <summary>
        /// Configures swagger generator for this API
        /// </summary>
        public static void Configure(this SwaggerGenOptions c)
        {
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            foreach (var version in GetVersions())
            {
                c.SwaggerDoc(
                version,
                new OpenApiInfo
                {
                    Title = "Product API example",
                    Version = version
                });
            }

            c.OperationFilter<RemoveVersionParameterFilter>();
            c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();

            c.DocInclusionPredicate((version, desc) =>
            {
                var versions = desc.ActionDescriptor.EndpointMetadata
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                var maps = desc.ActionDescriptor.EndpointMetadata
                    .OfType<MapToApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions)
                    .ToArray();

                return versions.Any(v => $"v{v}" == version)
                              && (!maps.Any() || maps.Any(v => $"v{v}" == version)); ;

            });
            c.EnableAnnotations();
        }

        /// <summary>
        /// Configures swagger UI for this API
        /// </summary>
        public static void Configure(this SwaggerUIOptions c)
        {
            foreach (var v in GetVersions())
            {
                c.SwaggerEndpoint($"/swagger/{v}/swagger.json", $"Product API {v}");
            }
        }

        /// <summary>
        /// Gets chronologicaly sorted available versions
        /// </summary>
        private static IEnumerable<string> GetVersions()
        {
            yield return "v1.0";
            yield return "v2.0";
        }
    }
}
