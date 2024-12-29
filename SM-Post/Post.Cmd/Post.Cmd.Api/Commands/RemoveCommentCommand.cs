using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record RemoveCommentCommand : CommandBase
{
    public Guid CommentId { get; set; }
    public string UserName { get; set; }
}

