﻿using Post.Common.Events;

namespace Post.Query.Infrastructure.Handlers;
public interface IEventHandler
{
    Task On(PostCreatedEvent @event);
    Task On(PostLikedEvent @event);
    Task On(MessageUpdatedEvent @event);
    Task On(PostRemovedEvent @event);
    Task On(CommentAddedEvent @event);
    Task On(CommentRemovedEvent @event);
    Task On(CommentUpdatedEvent @event);
}
