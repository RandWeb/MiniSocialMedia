using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;
internal sealed class EventSourcingHandler(IEventStore eventStore) : IEventSourcingHandler<PostAggregate>
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

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await eventStore.SaveEventAsync(aggregate.Id, aggregate.Version, [.. aggregate.GetUncommittedEvents]);
        aggregate.ClearEvents();
    }
}
