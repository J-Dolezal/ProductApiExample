using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductApiExample.ServiceLayer;

namespace ProductApiExample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(o => {
                o.AllowEmptyInputInBodyModelBinding = true;
            });
            services.AddAutoMapper(cfg => cfg.CreateMap<DataLayer.Entities.Product, Dto.Product>());
            services.AddDbContext<DataLayer.Context>(options => {
                options.UseSqlServer(Configuration.GetConnectionString(DataLayer.Context.ConnectionStringName));
            });
            services.AddServiceLayer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //todo: use filter, catching exceptions and turning them into 500 http code
        }
    }
}
