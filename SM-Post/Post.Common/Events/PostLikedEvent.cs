using CQRS.Core.Events;

namespace Post.Common.Events;

public record PostLikedEvent(Guid Id) : EventBase(Id,nameof(PostLikedEvent));
