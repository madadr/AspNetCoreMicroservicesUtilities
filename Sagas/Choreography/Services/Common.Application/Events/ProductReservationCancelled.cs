using System;

namespace Common.Application.Events
{
    public class ProductReservationCancelled : IEvent
    {
        public Guid OrderId { get; }
        public int ProductId { get; }
        public string Reason { get; }

        public ProductReservationCancelled(Guid orderId, int productId, string reason)
        {
            OrderId = orderId;
            ProductId = productId;
            Reason = reason;
        }
    }
}