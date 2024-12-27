using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events;

public class EventModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; init; }
    public DateTime TimeStamp { get; init; }
    public Guid AggregateIdentifier { get; init; }
    public string AggregateType { get; init; }
    public string EventType { get; init; }
    public EventBase EventData { get; init; }
    public int Version { get; init; }

    public EventModel()
    {
        
    }
    public EventModel(
        string id,
        DateTime timeStamp,
        Guid aggregateIdentifier,
        string aggregateType,
        string eventType,
        EventBase eventData,
        int version)
    {
        Id = id;
        TimeStamp = timeStamp;
        AggregateIdentifier = aggregateIdentifier;
        AggregateType = aggregateType;
        EventType = eventType;
        EventData = eventData;
        Version = version;
    }
}
