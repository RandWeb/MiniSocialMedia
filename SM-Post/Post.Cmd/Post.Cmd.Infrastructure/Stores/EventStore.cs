using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Stores;
internal sealed class EventStore(
    IEventStoreRepository eventStoreRepository,
    IEventProducer producer) : IEventStore
{
    public async Task<List<EventBase>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await eventStoreRepository.GetByAggregateIdAsync(aggregateId);

        if (eventStream is null or []) throw new AggregateNotFoundException("incorrect postId provided");

        return eventStream.OrderBy(p => p.Version).Select(e => e.EventData).ToList();
    }

    public async Task SaveEventAsync(Guid aggregateId, int expectedVersion, params EventBase[] events)
    {
        var eventStream = await eventStoreRepository.GetByAggregateIdAsync(aggregateId);

        if (expectedVersion != -1 && eventStream.Last().Version != expectedVersion)
        {
            throw new ConcurrencyException("Concurrency conflict");
        }

        var version = expectedVersion;
        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event,
                TimeStamp = DateTime.Now
            };

            await eventStoreRepository.SaveAsync(eventModel);
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? throw new Exception(" 'KAFKA_TOPIC' not founded in envirenment vatiables");

            await producer.ProduceAsync(topic, @event);
        }
    }
}
