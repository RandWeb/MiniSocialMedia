using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record UpdateCommentCommand(Guid CommentId, string Comment, string UserName) : CommandBase;

