using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.EventBus;
using Common.Application.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Store.Core.Entities;
using Store.Core.Repositories;

namespace Store.Application.Commands.Handlers
{
    public class BookProductCommandHandler : CommandHandlerBase<BookProduct>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IReservationRepository _repository;

        public BookProductCommandHandler(IMessageBroker messageBroker, IReservationRepository repository,
            ILogger<BookProductCommandHandler> logger) : base(logger)
        {
            _messageBroker = messageBroker;
            _repository = repository;
        }

        public override async Task HandleAsync(BookProduct command)
        {
            await base.LogHandleAsync(command);
            if (command.ProductId > 0 && await _repository.GetAsync(command.ProductId) == null)
            {
                await _repository.AddAsync(new Reservation(command.OrderId, command.ClientId, command.ProductId));
                await _messageBroker.PublishAsync(new ProductReserved(command.OrderId, command.ClientId,
                    command.ProductId, command.Price));
            }
            else
            {
                await _messageBroker.PublishAsync(new ProductReservationFailed(command.OrderId, command.ProductId));
            }
        }
    }
}