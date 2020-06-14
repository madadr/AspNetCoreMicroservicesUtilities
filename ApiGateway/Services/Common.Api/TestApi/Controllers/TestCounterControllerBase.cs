using Common.Application.TestApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.Api.TestApi.Controllers
{
    public class TestCounterControllerBase<T> : ControllerBase where T : ICounterMarker
    {
        private readonly ILogger<TestCounterControllerBase<T>> _logger;
        private readonly ICounter<T> _counter;

        public TestCounterControllerBase(ILogger<TestCounterControllerBase<T>> logger, ICounter<T> counter)
        {
            _logger = logger;
            _counter = counter;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var value = _counter.Value;
            _logger.LogDebug($"{typeof(TestCounterControllerBase<T>).FullName} value = {value}");
            return Ok(new {value = value});
        }

        [HttpDelete]
        public IActionResult Reset()
        {
            var value = _counter.Value;
            _logger.LogDebug($"Resetting {typeof(TestCounterControllerBase<T>).FullName} at value = {value}");
            _counter.Reset();
            return Ok();
        }
    }
}