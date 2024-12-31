using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Settings;

namespace Post.Cmd.Infrastructure.Repositories;
internal sealed class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository(IOptions<MongoDbSettings> options)
    {
        MongoClient client = new($"{options.Value.Host}:{options.Value.Port}");
        var mongoDatabase = client.GetDatabase(options.Value.DatabaseName);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(options.Value.CollectionName);
    }

    public async Task<List<EventModel>> FinAllAsync()
    {
       return await _eventStoreCollection.Find(_=>true).ToListAsync().ConfigureAwait(false);
    }

    public async Task<List<EventModel>> GetByAggregateIdAsync(Guid aggregateId)
    {
        return await _eventStoreCollection.Find(e => e.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
    }

    public async Task SaveAsync(EventModel eventModel)
    {
        await _eventStoreCollection.InsertOneAsync(eventModel).ConfigureAwait(false);
    }
}
