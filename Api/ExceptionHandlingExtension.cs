using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ProductApiExample.Api
{
    public static class ExceptionHandlingExtension
    {
        public static void UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetService<ILoggerFactory>().CreateLogger("API");

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {                        
                        logger.LogError(contextFeature.Error, "Exception occurred");
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
                        {
                            Status = context.Response.StatusCode,
                            Title = "Internal Server Error."
                        }));
                    }
                });
            });
        }
    }
}
