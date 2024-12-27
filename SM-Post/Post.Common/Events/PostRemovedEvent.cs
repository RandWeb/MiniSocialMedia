using CQRS.Core.Events;

namespace Post.Common.Events;

public record PostRemovedEvent(Guid Id) : EventBase(Id,nameof(PostRemovedEvent));
