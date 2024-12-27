using CQRS.Core.Events;

namespace Post.Common.Events;

public record CommentAddedEvent(
    Guid PostId,
    Guid CommentId,
    string Comment,
    string UserName,
    DateTime CommentDateTime
    ) : EventBase(PostId, nameof(CommentAddedEvent));
