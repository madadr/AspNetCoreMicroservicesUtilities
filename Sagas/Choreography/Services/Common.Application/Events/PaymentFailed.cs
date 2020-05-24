using System;

namespace Common.Application.Events
{
    public class PaymentFailed : IEvent
    {
        public Guid OrderId { get; }
        public int ClientId { get; }
        public int ProductId { get; }
        public string Reason { get; }

        public PaymentFailed(Guid orderId, int clientId, int productId, string reason = "PaymentFailed")
        {
            OrderId = orderId;
            ClientId = clientId;
            ProductId = productId;
            Reason = reason;
        }
    }
}