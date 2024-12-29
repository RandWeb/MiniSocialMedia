using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events;

public class EventModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public DateTime TimeStamp { get; set; }
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid AggregateIdentifier { get; set; }
    public string AggregateType { get; set; }
    public string EventType { get; set; }
    public EventBase EventData { get; set; }
    public int Version { get; set; }

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
