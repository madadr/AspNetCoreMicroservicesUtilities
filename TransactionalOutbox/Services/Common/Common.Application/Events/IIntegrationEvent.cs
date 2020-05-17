using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Application.Events
{
    public interface IIntegrationEvent
    {
        [BsonId] public Guid Id { get; }
    }
}