using System;
using Common.Application.Commands;

namespace Orders.Application.Commands
{
    public class PlaceOrder : ICommand
    {
        public Guid Id { get; }
        public int ClientId { get; }
        public int ProductId { get; }
        public double Price { get; }
        public DateTime? PlacedAt { get; }

        public PlaceOrder()
        {
        }

        public PlaceOrder(Guid id, int clientId, int productId, double price, DateTime? placedAt)
        {
            Id = id;
            ClientId = clientId;
            ProductId = productId;
            Price = price;
            PlacedAt = placedAt;
        }
    }
}