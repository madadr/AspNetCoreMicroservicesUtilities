using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.EventBus;
using Common.Application.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Payments.Application.Commands.Handlers
{
    public class ChargePaymentCommandHandler : CommandHandlerBase<ChargePayment>
    {
        private readonly IMessageBroker _messageBroker;

        public ChargePaymentCommandHandler(IMessageBroker messageBroker,
            ILogger<ChargePaymentCommandHandler> logger) : base(logger)
        {
            _messageBroker = messageBroker;
        }

        public override async Task HandleAsync(ChargePayment command)
        {
            await base.LogHandleAsync(command);
            if (command.Price > 0)
            {
                await _messageBroker.PublishAsync(new PaymentAccepted(command.OrderId, command.ClientId,
                    command.Price));
            }
            else
            {
                await _messageBroker.PublishAsync(new PaymentFailed(command.OrderId, command.ClientId,
                    command.ProductId));
            }
        }
    }
}