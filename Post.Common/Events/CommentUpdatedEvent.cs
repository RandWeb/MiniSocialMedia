using CQRS.Core.Events;

namespace Post.Common.Events;

public record CommentUpdatedEvent(Guid PostId, Guid CommentId, string Comment, string UserName, DateTime EditDateTime) : EventBase(PostId, nameof(CommentUpdatedEvent));
