using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates;
public sealed class PostAggregate : AggregateRoot
{
    private readonly Dictionary<Guid, Tuple<string, string>> _comments = [];
    public string Author { get; private set; }
    public bool IsActive { get; private set; }
    public PostAggregate()
    {
    }

    public PostAggregate(Guid id, string author, string message)
    {
        RaiseEvent(new PostCreatedEvent(id, author, message, DateTime.Now));
    }


    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        Author = @event.Author;
        IsActive = true;
    }

    public void UpdateMessage(string message)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("you cannot edit an message for an inactive post");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException($"the value of {message} cannot be null or empty. please provide a valid {message}");
        }

        RaiseEvent(new MessageUpdatedEvent(_id, message));
    }


    public void Apply(MessageUpdatedEvent @event)
    {
        _id = @event.Id;

    }

    public void LikePost()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("you cannot like an inactive post");
        }
        RaiseEvent(new PostLikedEvent(_id));
    }


    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }

    public void AddComment(string comment, string userName)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("you cannot add a comment to an inactive post");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"the value of {nameof(comment)} cannot be null or empty. please provide a valid {nameof(comment)}");
        }

        RaiseEvent(new CommentAddedEvent(_id, Guid.NewGuid(), comment, userName, DateTime.Now));
    }

    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.PostId;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
    }

    public void UpdateComment(Guid commentId, string comment, string userName)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("you cannot edit a comment for an inactive post");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"the value of {nameof(comment)} cannot be null or empty. please provide a valid {nameof(comment)}");
        }

        if (_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException($"You are not allowed to edit a comment that was made by another user!");
        }

        RaiseEvent(new CommentUpdatedEvent(_id, commentId, comment, userName, DateTime.Now));
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.PostId;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.UserName);
    }

    public void RemovedComment(Guid commentId, string userName)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("you cannot remove a comment for an inactive post");
        }


        if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException($"You are not allowed to remove a comment that was made by another user!");
        }


        RaiseEvent(new CommentRemovedEvent(_id, commentId));
    }

    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.PostId;
        _comments.Remove(@event.CommentId);
    }


    public void RemovePost(string username)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("you cannot delete an inactive post");
        }

        if (Author.Equals(username, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new InvalidOperationException($"You not allowed to delete a post that made was someone else");
        }
        RaiseEvent(new PostRemovedEvent(_id));
    }

    public void Apply(PostRemovedEvent @event)
    {
        _id = @event.Id;
        IsActive = false;
    }

}