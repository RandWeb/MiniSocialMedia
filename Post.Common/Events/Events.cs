using CQRS.Core.Events;

namespace Post.Common.Events;

public record CommentAddedEvent(
    Guid CommentId,
    string Comment,
    string UserName,
    DateTime CommentDateTime
    ) : EventBase(nameof(CommentAddedEvent));

public record CommentRemovedEvent(Guid CommentId) : EventBase(nameof(CommentRemovedEvent));

public record CommentUpdatedEvent(Guid CommentId, string Comment, string UserName, DateTime EditDateTime) : EventBase(nameof(CommentUpdatedEvent));

public record MessageUpdatedEvent(string Message) : EventBase(nameof(MessageUpdatedEvent));

public record PostCreatedEvent(string Author, string Message) : EventBase(nameof(PostCreatedEvent));

public record PostRemovedEvent() : EventBase(nameof(PostRemovedEvent));

public record PostLikedEvent() : EventBase(nameof(PostLikedEvent));
