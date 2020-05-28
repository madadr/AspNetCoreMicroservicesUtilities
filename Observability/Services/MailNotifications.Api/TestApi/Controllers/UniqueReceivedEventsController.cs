using Common.Api.TestApi.Controllers;
using Common.Application.TestApi;
using MailNotifications.Application.TestApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MailNotifications.Api.TestApi.Controllers
{
    [ApiController]
    [Route("/test/[controller]")]
    public class UniqueReceivedEventsController : TestCounterControllerBase<IUniqueReceivedEvents>
    {
        public UniqueReceivedEventsController(
            ILogger<TestCounterControllerBase<IUniqueReceivedEvents>> logger,
            ICounter<IUniqueReceivedEvents> counter) :
            base(logger, counter)
        {
        }
    }
}