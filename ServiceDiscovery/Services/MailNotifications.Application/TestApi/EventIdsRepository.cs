using System;
using System.Collections.Generic;
using System.Threading;

namespace MailNotifications.Application.TestApi
{
    public class EventIdsRepository
    {
        private readonly HashSet<Guid> _eventIds = new HashSet<Guid>();

        public bool Contains(Guid guid) => _eventIds.Contains(guid);
        public void Add(Guid guid)
        {
            _eventIds.Add(guid);
        }
    }
}