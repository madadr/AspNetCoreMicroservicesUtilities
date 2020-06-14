using System;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

namespace Common.Api.Extensions.HealthChecks.v1
{
    public class MongoDbServiceHealthChecker : IServiceHealthChecker
    {
        private readonly IMongoClient _mongoClient;
        private string _unhealthyDetails;

        public MongoDbServiceHealthChecker(IMongoClient mongoClient) => _mongoClient = mongoClient;

        public string Name => "MongoDb";

        public bool IsHealthy
        {
            get
            {
                try
                {
                    var isConnected = _mongoClient.Cluster.Description.State == ClusterState.Connected;
                    Details = isConnected ? "" : "Not connected";
                    return isConnected;
                }
                catch (Exception e)
                {
                    Details = $"Error during connection check: {e.Message}";
                    return false;
                }
            }
        }

        public string Details
        {
            get => IsHealthy ? "Healthy" : _unhealthyDetails;
            private set => _unhealthyDetails = value;
        }
    }
}
