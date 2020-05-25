using System;
using System.Threading.Tasks;
using Chronicle;
using Common.Application.Commands;
using Common.Application.EventBus;
using Common.Application.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orders.Application.Commands;
using Orders.Core.Entities;
using Orders.Core.Repositories;

namespace Orders.Api.Sagas
{
    // TODO: Use SagaData to store data
    public class CreateOrderSaga : Saga<SagaData>, ISagaStartAction<PlaceOrder>,
        ISagaAction<ClientVerified>, ISagaAction<ClientNotPermitted>, ISagaAction<ProductReserved>,
        ISagaAction<ProductReservationFailed>, ISagaAction<PaymentAccepted>, ISagaAction<PaymentFailed>
    {
        private readonly ILogger<CreateOrderSaga> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;

        public CreateOrderSaga(ILogger<CreateOrderSaga> logger, IOrderRepository orderRepository,
            IMessageBroker messageBroker)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
        }

        public override SagaId ResolveId(object message, ISagaContext context)
            => message switch
            {
                PlaceOrder m => (SagaId) m.Id.ToString(),
                ClientVerified m => (SagaId) m.OrderId.ToString(),
                ClientNotPermitted m => (SagaId) m.OrderId.ToString(),
                ProductReserved m => (SagaId) m.OrderId.ToString(),
                ProductReservationFailed m => m.OrderId.ToString(),
                PaymentAccepted m => m.OrderId.ToString(),
                PaymentFailed m => m.OrderId.ToString(),
                _ => base.ResolveId(message, context)
            };

        private void LogHandleCommand<T>(T message) where T : ICommand =>
            _logger.LogInformation($"Handling {message}: {JsonConvert.SerializeObject(message)}");

        private void LogCompensateCommand<T>(T message) where T : ICommand =>
            _logger.LogInformation($"Compensating {message}: {JsonConvert.SerializeObject(message)}");

        private void LogHandleEvent<T>(T message) where T : IEvent =>
            _logger.LogInformation($"Handling {message}: {JsonConvert.SerializeObject(message)}");

        private void LogCompensateEvent<T>(T message) where T : IEvent =>
            _logger.LogInformation($"Compensating {message}: {JsonConvert.SerializeObject(message)}");

        public async Task HandleAsync(PlaceOrder command, ISagaContext context)
        {
            LogHandleCommand(command);
            if (command.ProductId > 0)
            {
                await _orderRepository.AddAsync(new Order(command.Id, command.ClientId, command.ProductId,
                    command.Price, placedAt: command.PlacedAt));
                await _messageBroker.PublishCommandAsync(new VerifyClient(command.Id, command.ClientId,
                    command.ProductId,
                    command.Price));
            }
            else
            {
                Reject();
            }
        }

        public async Task CompensateAsync(PlaceOrder command, ISagaContext context)
        {
            LogCompensateCommand(command);

            var order = await _orderRepository.GetAsync(command.Id);
            if (order != null)
            {
                order.State = OrderState.Cancelled;
                order.CancelledAt = DateTime.Now;
                await _orderRepository.UpdateAsync(order);
            }
        }

        public async Task HandleAsync(ClientVerified @event, ISagaContext context)
        {
            LogHandleEvent(@event);
            await _messageBroker.PublishCommandAsync(new BookProduct(@event.OrderId, @event.ClientId, @event.ProductId,
                @event.Price));
        }

        public async Task CompensateAsync(ClientVerified @event, ISagaContext context) => await Task.CompletedTask;

        public async Task HandleAsync(ClientNotPermitted @event, ISagaContext context)
        {
            LogHandleEvent(@event);
            await RejectAsync();
        }

        public async Task CompensateAsync(ClientNotPermitted @event, ISagaContext context) => await Task.CompletedTask;

        public async Task HandleAsync(ProductReserved @event, ISagaContext context)
        {
            LogHandleEvent(@event);
            await _messageBroker.PublishCommandAsync(new ChargePayment(@event.OrderId, @event.ClientId,
                @event.ProductId,
                @event.Price));
        }

        public async Task CompensateAsync(ProductReserved @event, ISagaContext context)
        {
            LogCompensateEvent(@event);
            await _messageBroker.PublishCommandAsync(new CancelProductBooking(@event.OrderId, @event.ProductId,
                "ProductReservationFailed"));
        }

        public async Task HandleAsync(ProductReservationFailed @event, ISagaContext context)
        {
            LogHandleEvent(@event);
            await RejectAsync();
        }

        public async Task CompensateAsync(ProductReservationFailed @event, ISagaContext context) =>
            await Task.CompletedTask;

        public async Task HandleAsync(PaymentAccepted @event, ISagaContext context)
        {
            LogHandleEvent(@event);
            _logger.LogInformation($"Approving order {@event.OrderId}");
            var order = await _orderRepository.GetAsync(@event.OrderId);
            order.State = OrderState.Approved;
            order.ApprovedAt = DateTime.Now;
            await _orderRepository.UpdateAsync(order);
            await CompleteAsync();
        }

        public async Task CompensateAsync(PaymentAccepted @event, ISagaContext context) => await Task.CompletedTask;

        public async Task HandleAsync(PaymentFailed @event, ISagaContext context)
        {
            LogHandleEvent(@event);
            await RejectAsync();
        }

        public async Task CompensateAsync(PaymentFailed @event, ISagaContext context) => await Task.CompletedTask;
    }

    public class SagaData
    {
        public SagaData()
        {
        }
    }
}