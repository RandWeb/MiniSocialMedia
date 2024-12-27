using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure;
public interface IEventStore
{
    Task SaveEventAsync(Guid aggregateId, int expectedVersion, params EventBase[] events);
    Task<List<EventBase>> GetEventsAsync(Guid aggregateId);

}
