using CQRS.Core.Events;

namespace Post.Common.Events;

public record CommentUpdatedEvent(Guid PostId, Guid CommentId, string Comment, string UserName, DateTime Modified_At) : EventBase(PostId, nameof(CommentUpdatedEvent));
