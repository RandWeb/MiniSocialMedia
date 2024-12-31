
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands;

public sealed class CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcing) : ICommandHandler
{

    public async Task HandleAsync(AddPostCommand command)
    {
        var aggregate = new PostAggregate(command.Id, command.Author, command.Message);

        await eventSourcing.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RemovePostCommand command)
    {
        var aggregate = await eventSourcing.GetByIdAsync(command.Id);

        aggregate.RemovePost(command.UserName);

        await eventSourcing.SaveAsync(aggregate);
    }

    public async Task HandleAsync(LikePostCommand command)
    {
        var aggregate = await eventSourcing.GetByIdAsync(command.Id);

        aggregate.LikePost();

        await eventSourcing.SaveAsync(aggregate);
    }

    public async Task HandleAsync(AddCommentCommand command)
    {
        var aggregate = await eventSourcing.GetByIdAsync(command.Id);

        aggregate.AddComment(command.Comment, command.UserName);

        await eventSourcing.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RemoveCommentCommand command)
    {
        var aggregate = await eventSourcing.GetByIdAsync(command.Id);

        aggregate.RemovedComment(command.CommentId, command.UserName);

        await eventSourcing.SaveAsync(aggregate);
    }

    public async Task HandleAsync(UpdateCommentCommand command)
    {
        var aggregate = await eventSourcing.GetByIdAsync(command.Id);

        aggregate.UpdateComment(command.CommentId, command.Comment, command.UserName);

        await eventSourcing.SaveAsync(aggregate);
    }

    public async Task HandleAsync(UpdateMessageCommand command)
    {
        var aggregate = await eventSourcing.GetByIdAsync(command.Id);

        aggregate.UpdateMessage(command.Message);

        await eventSourcing.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RestoreDbCommand command)
    {
        await eventSourcing.RepublishEventsAsync();
    }
}
