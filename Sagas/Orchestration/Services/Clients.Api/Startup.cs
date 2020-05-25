using Clients.Application.Commands.Handlers;
using Common.Api.Extensions;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Clients.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEventBus();
            services.AddTransient(typeof(ICommandHandler<VerifyClient>), typeof(VerifyClientCommandHandler));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.SubscribeCommand<VerifyClient>(logger, "Clients");
        }
    }
}
