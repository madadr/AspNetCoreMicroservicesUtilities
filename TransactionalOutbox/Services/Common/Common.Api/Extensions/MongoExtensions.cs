using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Common.Api.Extensions
{
    public static class MongoExtensions
    {
        public static void AddMongo(this IServiceCollection services)
        {
            // TODO: Use IConfiguration
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            services.AddSingleton(typeof(IMongoClient), mongoClient);
            services.AddSingleton(typeof(IMongoDatabase), mongoClient.GetDatabase("IntegrationEvents"));
        }
    }
}