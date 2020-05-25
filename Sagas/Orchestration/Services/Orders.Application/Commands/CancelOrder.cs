using System;
using Common.Application.Commands;

namespace Orders.Application.Commands
{
    public class CancelOrder : ICommand
    {
        public Guid OrderId { get; }
        public string Reason { get; }
        public DateTime CancelledAt { get; }

        public CancelOrder(Guid orderId, string reason, DateTime cancelledAt)
        {
            OrderId = orderId;
            Reason = reason;
            CancelledAt = cancelledAt;
        }
    }
}