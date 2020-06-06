using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceRegistry.Repositories;

namespace ServiceRegistry.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ILogger<ServiceController> _logger;
        private readonly IInstanceRepository _repository;

        public ServiceController(ILogger<ServiceController> logger, IInstanceRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(ServiceInstanceData instanceData)
        {
            _logger.LogInformation($"RegisterAsync {JsonConvert.SerializeObject(instanceData)}");
            if (!isDataValid(instanceData))
            {
                return await Task.FromResult(BadRequest("Invalid instance data."));
            }

            if (_repository.Contains(instanceData.InstanceId))
            {
                return await Task.FromResult(Ok("Already registered"));
            }

            _repository.Add(instanceData);
            return await Task.FromResult(Ok("Registered"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> UnregisterAsync(string id)
        {
            _logger.LogInformation($"UnregisterAsync {id}");
            if (string.IsNullOrEmpty(id) || !_repository.Contains(id))
            {
                return await Task.FromResult(BadRequest("Invalid ID."));
            }

            _repository.Remove(id);
            return await Task.FromResult(Ok("Unregistered"));
        }

        [HttpGet]
        public async Task<IActionResult> GetStates()
        {
            _logger.LogInformation("GetStates");
            return await Task.FromResult(Ok(new {states = _repository.GetInstanceStates()}));
        }

        [HttpGet("{serviceName}")]
        public async Task<IActionResult> GetServiceInstances(string serviceName)
        {
            _logger.LogInformation($"GetServiceInstances {serviceName}");
            return await Task.FromResult(Ok(new {addresses = _repository.GetServiceAddresses(serviceName)}));
        }

        private bool isDataValid(ServiceInstanceData data)
            => !string.IsNullOrEmpty(data.ServiceName)
               && !string.IsNullOrEmpty(data.InstanceId)
               && !string.IsNullOrEmpty(data.InstanceAddress)
               && (!data.HasHealthCheck || !string.IsNullOrEmpty(data.HealthCheckAddress));
    }
}