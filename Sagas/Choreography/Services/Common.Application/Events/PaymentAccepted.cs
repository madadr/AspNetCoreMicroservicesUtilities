using System;

namespace Common.Application.Events
{
    public class PaymentAccepted : IEvent
    {
        public Guid OrderId { get; }
        public int ClientId { get; }
        public double Price { get; }

        public PaymentAccepted(Guid orderId, int clientId, double price)
        {
            OrderId = orderId;
            ClientId = clientId;
            Price = price;
        }
    }
}