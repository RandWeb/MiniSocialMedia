using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public interface ICommandHandler
{
    Task HandleAsync(AddPostCommand command);
    Task HandleAsync(RemovePostCommand command);
    Task HandleAsync(LikePostCommand command);
    Task HandleAsync(AddCommentCommand command);
    Task HandleAsync(RemoveCommentCommand command);
    Task HandleAsync(UpdateCommentCommand command);
    Task HandleAsync(UpdateMessageCommand command);


}
