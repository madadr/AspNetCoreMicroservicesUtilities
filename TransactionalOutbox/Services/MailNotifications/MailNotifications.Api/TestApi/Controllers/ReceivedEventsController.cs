using Common.Api.TestApi.Controllers;
using Common.Application.TestApi;
using MailNotifications.Application.TestApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MailNotifications.Api.TestApi.Controllers
{
    [ApiController]
    [Route("/test/[controller]")]
    public class ReceivedEventsController : TestCounterControllerBase<IReceivedEvents>
    {
        public ReceivedEventsController(ILogger<TestCounterControllerBase<IReceivedEvents>> logger,
            ICounter<IReceivedEvents> counter) :
            base(logger, counter)
        {
        }
    }
}