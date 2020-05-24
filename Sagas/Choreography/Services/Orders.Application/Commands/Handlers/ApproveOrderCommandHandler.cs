using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Microsoft.Extensions.Logging;
using Orders.Core.Entities;
using Orders.Core.Repositories;

namespace Orders.Application.Commands.Handlers
{
    public class ApproveOrderCommandHandler : CommandHandlerBase<ApproveOrder>
    {
        private readonly IOrderRepository _orderRepository;

        public ApproveOrderCommandHandler(IOrderRepository orderRepository,
            ILogger<ApproveOrderCommandHandler> logger) : base(logger)
        {
            _orderRepository = orderRepository;
        }

        public override async Task HandleAsync(ApproveOrder command)
        {
            await base.LogHandleAsync(command);
            var order = await _orderRepository.GetAsync(command.OrderId);
            order.State = OrderState.Approved;
            order.ApprovedAt = command.ApprovedAt;
            await _orderRepository.UpdateAsync(order);
        }
    }
}