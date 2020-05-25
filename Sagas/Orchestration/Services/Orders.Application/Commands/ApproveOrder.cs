using System;
using Common.Application.Commands;

namespace Orders.Application.Commands
{
    public class ApproveOrder : ICommand
    {
        public Guid OrderId { get; }
        public DateTime? ApprovedAt { get; }

        public ApproveOrder(Guid orderId, DateTime? approvedAt)
        {
            OrderId = orderId;
            ApprovedAt = approvedAt;
        }
    }
}