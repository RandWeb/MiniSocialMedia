using CQRS.Core.Events;

namespace Post.Common.Events;

public record PostCreatedEvent(Guid Id,string Author, string Message,DateTime DatePosted) : EventBase(Id, nameof(PostCreatedEvent));
