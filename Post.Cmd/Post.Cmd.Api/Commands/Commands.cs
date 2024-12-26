using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record AddCommentCommand(
    string Comment,
    string UserName
    ) : CommandBase;

public record DeletePostCommand(string UserName) : CommandBase;

public record EditCommentCommand(Guid CommentId, string Comment, string UserName) : CommandBase;

public record EditMessageCommand(string Message) : CommandBase;

public record LikePostCommand : CommandBase;

public record NewPostCommand(string Author, string Message) : CommandBase;

public record RemoveCommentCommand(Guid CommentId, string UserName) : CommandBase;

