using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Application.EventBus;
using Common.Application.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Store.Core.Repositories;

namespace Store.Application.Commands.Handlers
{
    public class CancelProductBookingCommandHandler : CommandHandlerBase<CancelProductBooking>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IReservationRepository _repository;

        public CancelProductBookingCommandHandler(IMessageBroker messageBroker, IReservationRepository repository,
            ILogger<CancelProductBookingCommandHandler> logger) : base(logger)
        {
            _messageBroker = messageBroker;
            _repository = repository;
        }

        public override async Task HandleAsync(CancelProductBooking command)
        {
            await base.LogHandleAsync(command);
            if (await _repository.GetAsync(command.ProductId) != null)
            {
                await _repository.RemoveAsync(command.ProductId);
            }
            await _messageBroker.PublishAsync(new ProductReservationCancelled(command.OrderId, command.ProductId, command.Reason));
        }
    }
}