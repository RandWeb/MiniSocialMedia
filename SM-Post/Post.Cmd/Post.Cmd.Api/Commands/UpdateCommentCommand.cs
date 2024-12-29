using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record UpdateCommentCommand : CommandBase
{

    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public string UserName { get; set; }
}

