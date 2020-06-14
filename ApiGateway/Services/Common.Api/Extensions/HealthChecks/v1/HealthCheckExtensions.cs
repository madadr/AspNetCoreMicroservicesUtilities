using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Common.Api.Extensions.HealthChecks.v1
{
    public static class HealthCheckExtensions
    {
        public static void MapCommonHealthChecks(this IEndpointRouteBuilder endpoints, IApplicationBuilder app)
        {
            endpoints.MapGet("/hc", async context =>
            {
                context.Response.ContentType = "application/json";
                var serviceHealthCheckers = app.ApplicationServices.GetService<IEnumerable<IServiceHealthChecker>>();
                var infos = serviceHealthCheckers.Select(checker => new ServiceHealthInfo
                    {Name = checker.Name, Healthy = checker.IsHealthy, Details = checker.Details});
                context.Response.StatusCode = infos.Any(info => !info.Healthy)
                    ? StatusCodes.Status500InternalServerError : StatusCodes.Status200OK;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(infos));
            });
        }

        private struct ServiceHealthInfo
        {
            public string Name;
            public bool Healthy;
            public string Details;
        }
    }
}