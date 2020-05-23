using System;

namespace Common.Application.Events
{
    public class PaymentFailed : IEvent
    {
        public Guid OrderId { get; }
        public string Reason { get; }

        public PaymentFailed(Guid orderId, string reason)
        {
            OrderId = orderId;
            Reason = reason;
        }
    }
}