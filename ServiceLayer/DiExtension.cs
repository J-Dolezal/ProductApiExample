using Microsoft.Extensions.DependencyInjection;
using ProductApiExample.ServiceLayer.Services;

namespace ProductApiExample.ServiceLayer
{
    public static class DiExtension
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
