using System;
using Common.Application.Commands;

namespace Store.Application.Commands
{
    public class CancelProductBooking : ICommand
    {
        public Guid OrderId { get; }
        public int ProductId { get; }
        public string Reason { get; }

        public CancelProductBooking(Guid orderId, int productId, string reason)
        {
            OrderId = orderId;
            ProductId = productId;
            Reason = reason;
        }
    }
}