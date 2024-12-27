using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record RemoveCommentCommand(Guid CommentId, string UserName) : CommandBase;

