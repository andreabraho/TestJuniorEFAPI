using DataLayer.Interfaces;
using DataLayer.Repository;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestJuniorEFAPI
{
    public class Startup
    {
        IWebHostEnvironment _env;
        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContextPool<MyContext>(optionsBuilder =>
            {
                string ConnectionString = Configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlServer(ConnectionString).EnableSensitiveDataLogging(_env.IsDevelopment());
            });

        



        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IInfoRequestRepository, InfoRequestRepository>();

            services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
