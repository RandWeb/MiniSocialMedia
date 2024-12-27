using CQRS.Core.Messages;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events;
public abstract record EventBase(Guid Id,string Type) : MessageBase(Id)
{
    public int Version { get; set; }
}
