using System;

namespace Users
{
    public class InstanceIdHolder
    {
        public Guid Id { get; }

        public InstanceIdHolder(Guid id)
        {
            Id = id;
        }
    }
}