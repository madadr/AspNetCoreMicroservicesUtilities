using Common.Application.TestApi;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Api.Extensions
{
    public static class TestApiExtensions
    {
        public static void AddCounter<T>(this IServiceCollection services) where T : ICounterMarker
        {
            services.AddSingleton(typeof(ICounter<T>), typeof(Counter<T>));
        }
    }
}