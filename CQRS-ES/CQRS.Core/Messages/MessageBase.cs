using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CQRS.Core.Messages;
public abstract record MessageBase
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    protected MessageBase(Guid id)
    {
        Id = id;
    }
    protected MessageBase()
    {
        
    }

}
