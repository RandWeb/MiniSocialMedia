using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;
internal sealed class EventSourcingHandler(IEventStore eventStore,IEventProducer eventProducer) : IEventSourcingHandler<PostAggregate>
{
    public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
    {
        var postAggregate = new PostAggregate();

        var events = await eventStore.GetEventsAsync(aggregateId);

        if (events.Count == 0) return postAggregate;

        postAggregate.ReplayEvent([.. events]);

        postAggregate.Version = events.Max(p => p.Version);

        return postAggregate;
    }

    public async Task RepublishEventsAsync()
    {
        var aggregateIds = await eventStore.GetAggregateIdsAsync();

        if (aggregateIds?.Count is 0 or null) return;

        foreach (var aggregateId in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aggregateId);

            if (aggregate?.IsActive is null or false) continue;

            var events = await eventStore.GetEventsAsync(aggregateId);

            foreach (var @event in events)
            {
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? throw new Exception(" 'KAFKA_TOPIC' not founded in envirenment vatiables");
                    
                await eventProducer.ProduceAsync(topic, @event);
            }
        }
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await eventStore.SaveEventAsync(aggregate.Id, aggregate.Version, [.. aggregate.GetUncommittedEvents]);
        aggregate.ClearEvents();
    }
}
