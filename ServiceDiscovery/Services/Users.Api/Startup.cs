using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Common.Api.Extensions;
using Microsoft.Extensions.Logging;

namespace Users
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(new InstanceIdHolder(Guid.NewGuid()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime,
            ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceRegistry(lifetime, logger, new ServiceInstanceData()
            {
                ServiceName = "users_service",
                InstanceId = app.ApplicationServices.GetService<InstanceIdHolder>().Id.ToString(),
                InstanceAddress = _configuration["serviceRegistry:instanceAddress"],
                HasHealthCheck = true,
                HealthCheckAddress = "/healthcheck"
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}