using System;

namespace Common.Application.Events
{
    public class ProductReservationFailed : IEvent
    {
        public Guid OrderId { get; }
        public int ProductId { get; }
        public string Reason { get; }

        public ProductReservationFailed(Guid orderId, int productId, string reason = "ProductReservationFailed")
        {
            OrderId = orderId;
            ProductId = productId;
            Reason = reason;
        }
    }
}