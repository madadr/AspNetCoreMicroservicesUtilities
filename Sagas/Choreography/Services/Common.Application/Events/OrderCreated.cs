using System;

namespace Common.Application.Events
{
    public class OrderCreated : IEvent
    {
        public Guid Id { get; }
        public int ClientId { get; }
        public int ProductId { get; }
        public double Price { get; }

        public OrderCreated(Guid id, int clientId, int productId, double price)
        {
            Id = id;
            ClientId = clientId;
            ProductId = productId;
            Price = price;
        }
    }
}