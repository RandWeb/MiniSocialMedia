using CQRS.Core.Events;

namespace CQRS.Core.Domain;
public abstract class AggregateRoot
{
    private readonly List<EventBase> _events = [];

    protected Guid _id;


    public Guid Id
    {
        get { return _id; }
    }


    public int Version { get; set; } = -1;

    public IReadOnlyList<EventBase> GetUncommittedEvents => _events;

    public void ClearEvents() => _events.Clear();

    private void ApplyEvent(EventBase @event, bool isNew)
    {
        var method = GetType().GetMethod("Apply", [@event.GetType()]);

        ArgumentNullException.ThrowIfNull(method, $"Missing Apply method for {@event.GetType().Name}");

        method.Invoke(this, [@event]);

        if (isNew)
        {
            _events.Add(@event);
        }
    }

    protected void RaiseEvent(EventBase @event)
    {
        ApplyEvent(@event, true);
    }

    public void ReplayEvent(params EventBase [] events)
    {
        foreach (var @event in events)
        {
            ApplyEvent(@event, false);
        }
    }
}
