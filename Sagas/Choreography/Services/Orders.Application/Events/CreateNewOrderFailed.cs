using System;
using Common.Application.Events;

namespace Orders.Application.Events
{
    public class CreateNewOrderFailed : IEvent
    {
        public Guid Id { get; }
        public string Reason { get; }

        public CreateNewOrderFailed(Guid id, string reason = "CreateNewOrderFailed")
        {
            Id = id;
            Reason = reason;
        }
    }
}