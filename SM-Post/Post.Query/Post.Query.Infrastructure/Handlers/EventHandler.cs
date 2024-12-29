using Microsoft.Extensions.Hosting;
using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

internal sealed class EventHandler : IEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    public async Task On(PostCreatedEvent @event)
    {
        var post = new PostEntity
        {
            PostId = @event.Id,
            Author = @event.Author,
            Message = @event.Message,
            Create_At = @event.Create_At
        };
        await _postRepository.CreateAsync(post);
    }
    public async Task On(PostLikedEvent @event)
    {
        var post = await _postRepository.GetPostByIdAsync(@event.Id);
        if (post is null) return;

        post.Likes++;
        await _postRepository.UpdateAsync(post);
    }
    public async Task On(MessageUpdatedEvent @event)
    {
        var post = await _postRepository.GetPostByIdAsync(@event.Id);
        if (post is null) return;
        post.Message = @event.Message;
        await _postRepository.UpdateAsync(post);
    }
    public async Task On(PostRemovedEvent @event)
    {
        await _postRepository.DeleteAsync(@event.Id);
    }
    public async Task On(CommentAddedEvent @event)
    {
        await _commentRepository.CreateAsync(new CommentEntity
        {
            UserName = @event.UserName,
            Created_At = @event.Created_At,
            CommentId = @event.CommentId,
            PostId = @event.PostId,
            Comment = @event.Comment,
            IsEdited = false
        });

    }
    public async Task On(CommentRemovedEvent @event)
    {
        await _commentRepository.RemoveAsync(@event.CommentId);
    }
    public async Task On(CommentUpdatedEvent @event)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(@event.CommentId);
        if (comment is null) return;

        comment.Comment = @event.Comment;
        comment.IsEdited = true;
        comment.UserName = @event.UserName;
        comment.Modified_At = @event.Modified_At;

        await _commentRepository.UpdateAsync(comment);
    }
}
