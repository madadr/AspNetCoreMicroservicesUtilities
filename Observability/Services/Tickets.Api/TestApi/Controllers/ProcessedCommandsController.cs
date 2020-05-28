using Common.Api.TestApi.Controllers;
using Common.Application.TestApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tickets.Application.TestApi;

namespace Tickets.Api.TestApi.Controllers
{
    [ApiController]
    [Route("/test/[controller]")]
    public class ProcessedCommandsController : TestCounterControllerBase<IProcessedCommands>
    {
        public ProcessedCommandsController(ILogger<TestCounterControllerBase<IProcessedCommands>> logger,
            ICounter<IProcessedCommands> counter) :
            base(logger, counter)
        {
        }
    }
}