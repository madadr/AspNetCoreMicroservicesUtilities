using System;

namespace Common.Application.Events
{
    public class ProductReserved : IEvent
    {
        public Guid OrderId { get; }
        public int ClientId { get; }
        public int ProductId { get; }
        public double Price { get; }

        public ProductReserved(Guid orderId, int clientId, int productId, double price)
        {
            OrderId = orderId;
            ClientId = clientId;
            ProductId = productId;
            Price = price;
        }
    }
}