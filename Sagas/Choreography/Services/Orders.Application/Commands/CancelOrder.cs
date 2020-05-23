using System;
using Common.Application.Commands;

namespace Orders.Application.Commands
{
    public class CancelOrder : ICommand
    {
        public Guid OrderId { get; }
        public string Reason { get; }

        public CancelOrder(Guid orderId, string reason)
        {
            OrderId = orderId;
            Reason = reason;
        }
    }
}