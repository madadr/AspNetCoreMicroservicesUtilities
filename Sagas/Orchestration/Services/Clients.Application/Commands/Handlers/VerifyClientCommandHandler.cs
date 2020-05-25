using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.EventBus;
using Common.Application.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Clients.Application.Commands.Handlers
{
    public class VerifyClientCommandHandler : CommandHandlerBase<VerifyClient>
    {
        private readonly IMessageBroker _messageBroker;

        public VerifyClientCommandHandler(IMessageBroker messageBroker,
            ILogger<VerifyClientCommandHandler> logger) : base(logger)
        {
            _messageBroker = messageBroker;
        }

        public override async Task HandleAsync(VerifyClient command)
        {
            await base.LogHandleAsync(command);
            if (command.ClientId > 0)
            {
                await _messageBroker.PublishAsync(new ClientVerified(command.OrderId, command.ClientId,
                    command.ProductId, command.Price));
            }
            else
            {
                await _messageBroker.PublishAsync(new ClientNotPermitted(command.OrderId, command.ClientId));
            }
        }
    }
}