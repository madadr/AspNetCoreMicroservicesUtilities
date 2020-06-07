using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Router.Routing;

namespace Router
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
            services.AddControllers();
            services.AddSingleton<Routing.Router>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SuperviseServiceRegistry(logger);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("{serviceName}/{action}", async context =>
                {
                    var serviceNameValue = context.GetRouteData().Values["serviceName"] as string;
                    var actionValue = context.GetRouteData().Values["action"] as string;
                    logger.LogInformation($"serviceName = {serviceNameValue}, action = {actionValue}");
                    if (string.IsNullOrEmpty(serviceNameValue))
                    {
                        await RouteNotFound(context);
                        return;
                    }

                    if (actionValue == null)
                    {
                        actionValue = "";
                    }

                    var routedPath = app.ApplicationServices.GetService<Routing.Router>().GetNextPath(serviceNameValue);
                    if (routedPath == null)
                    {
                        await RouteNotFound(context);
                        return;
                    }
                    var message = new HttpRequestMessage(new HttpMethod(context.Request.Method), $"{routedPath}/{actionValue}");
                    var response = await (new HttpClient().SendAsync(message));
                    await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
                });
            });
        }

        private static async Task RouteNotFound(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Route not found");
        }
    }
}