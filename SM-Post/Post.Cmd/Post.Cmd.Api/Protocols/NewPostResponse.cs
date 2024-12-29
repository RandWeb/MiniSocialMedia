using Post.Common.Protocols;

namespace Post.Cmd.Api.Protocols;

public record NewPostResponse : ResponseBase
{
    public Guid Id { get; set; }
}
