using System;
using Common.Application.Commands;

namespace Orders.Application.Commands
{
    public class PlaceOrder : ICommand
    {
        public Guid Id { get; }
        public int CustomerId { get; }
        public int ProductId { get; }
        public double Price { get; }

        public PlaceOrder(Guid id, int customerId, int productId, double price)
        {
            Id = id;
            CustomerId = customerId;
            ProductId = productId;
            Price = price;
        }
    }
}