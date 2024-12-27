using CQRS.Core.Events;

namespace Post.Common.Events;

public record CommentRemovedEvent(Guid PostId,Guid CommentId) : EventBase(PostId, nameof(CommentRemovedEvent));
