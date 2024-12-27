using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Settings;

namespace Post.Cmd.Infrastructure.Repositories;
internal class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository(IOptions<MongoDbSettings> options)
    {
        MongoClient client = new($"{options.Value.Host}{options.Value.Port}");
        _eventStoreCollection = client.GetDatabase(options.Value.DatabaseName).GetCollection<EventModel>(options.Value.CollectionName);

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
