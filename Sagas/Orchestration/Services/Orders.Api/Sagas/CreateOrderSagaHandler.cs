using System.Threading.Tasks;
using Chronicle;
using Common.Application.Commands.Handlers;
using Common.Application.Events;
using Common.Application.Events.Handlers;
using Orders.Application.Commands;

namespace Orders.Api.Sagas
{
    public class CreateOrderHandler : ICommandHandler<PlaceOrder>,
        IEventHandler<ClientVerified>, IEventHandler<ClientNotPermitted>,
        IEventHandler<ProductReserved>, IEventHandler<ProductReservationFailed>, IEventHandler<PaymentAccepted>,
        IEventHandler<PaymentFailed>
    {
        private readonly ISagaCoordinator _coordinator;

        public CreateOrderHandler(ISagaCoordinator coordinator) => _coordinator = coordinator;

        public async Task HandleAsync(PlaceOrder command) =>
            await _coordinator.ProcessAsync(command, SagaContext.Empty);

        public async Task HandleAsync(OrderCreated @event) =>
            await _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public async Task HandleAsync(CreateNewOrderFailed @event) =>
            await _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public async Task HandleAsync(ClientVerified @event) =>
            await _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public async Task HandleAsync(ClientNotPermitted @event) =>
            await _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public async Task HandleAsync(ProductReserved @event) =>
            await _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public async Task HandleAsync(ProductReservationFailed @event) =>
            await _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public async Task HandleAsync(PaymentAccepted @event) =>
            await _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public async Task HandleAsync(PaymentFailed @event) =>
            await _coordinator.ProcessAsync(@event, SagaContext.Empty);
    }
}