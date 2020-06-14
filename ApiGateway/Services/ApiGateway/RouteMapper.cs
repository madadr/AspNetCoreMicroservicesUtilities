using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiGateway
{
    public static class RouteMapper
    {
        public static void MapRoutes(this IEndpointRouteBuilder endpoints, RouteOption[] routes,
            ILogger<Startup> logger)
        {
            foreach (var route in routes)
            {
                logger.LogInformation($"Mapping route {JsonConvert.SerializeObject(route)}");
                endpoints.MapMethods(route.OriginRoute, new[] {route.Method.ToUpper()}, async context =>
                {
                    logger.LogInformation($"Handling route {JsonConvert.SerializeObject(route)}");
                    var newRequest = new HttpRequestMessage(new HttpMethod(context.Request.Method),
                        $"http://{route.DestinationHost}/{route.DestinationRoute}");
                    var contentType = context.Request.ContentType;
                    if (!string.IsNullOrEmpty(contentType))
                    {
                        using StreamReader streamReader = new StreamReader(context.Request.Body, Encoding.UTF8);
                        var body = await streamReader.ReadToEndAsync();
                        logger.LogDebug($"Body = {body}");
                        newRequest.Content = new StringContent(body, Encoding.UTF8, contentType);
                    }

                    var response = await new HttpClient {Timeout = TimeSpan.FromSeconds(2)}.SendAsync(newRequest);
                    await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
                });
            }
        }
    }
}