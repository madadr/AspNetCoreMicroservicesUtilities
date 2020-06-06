using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Tickets.Api.Controllers.Cssd
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        private static readonly string ServiceRegistryAddress = "http://service-registry:80/service";

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                await new HttpClient().GetAsync(await ResolveAddress("users_service") + "/users");
                return Ok();
            }
            catch
            {
                return BadRequest("Not resolved");
            }
        }

        private async Task<string> ResolveAddress(string serviceName)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(ServiceRegistryAddress + "/" + serviceName);
            var addresses = JsonConvert.DeserializeObject<ResponseBody>(await response.Content.ReadAsStringAsync())
                .Addresses;
            var addressesCount = addresses?.Count;
            if (addresses == null || addressesCount == 0)
            {
                throw new Exception();
            }

            var chosen = addresses[new Random().Next(0, addressesCount.Value)];
            _logger.LogInformation(
                $"Instances amount: {addressesCount}. Addresses: {JsonConvert.SerializeObject(addresses)}. Chosen: {chosen}");

            return chosen;
        }
    }

    internal struct ResponseBody
    {
        public IList<string> Addresses { get; set; }

        public ResponseBody(IList<string> addresses)
        {
            Addresses = addresses;
        }
    }
}