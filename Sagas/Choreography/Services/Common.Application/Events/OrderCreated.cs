using System;

namespace Common.Application.Events
{
    public class OrderCreated : IEvent
    {
        private readonly Guid _id;
        private readonly int _customerId;
        private readonly int _productId;
        private readonly double _price;

        public OrderCreated(Guid id, int customerId, int productId, double price)
        {
            _id = id;
            _customerId = customerId;
            _productId = productId;
            _price = price;
        }
    }
}