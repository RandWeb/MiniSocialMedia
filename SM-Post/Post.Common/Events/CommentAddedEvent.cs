using CQRS.Core.Events;

namespace Post.Common.Events;

public record CommentAddedEvent(
    Guid PostId,
    Guid CommentId,
    string Comment,
    string UserName,
    DateTime Created_At
    ) : EventBase(PostId, nameof(CommentAddedEvent));
