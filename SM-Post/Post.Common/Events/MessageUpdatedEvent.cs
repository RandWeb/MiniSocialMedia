using CQRS.Core.Events;

namespace Post.Common.Events;

public record MessageUpdatedEvent(Guid Id,string Message) : EventBase(Id,nameof(MessageUpdatedEvent));
