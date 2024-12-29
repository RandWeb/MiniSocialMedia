using Post.Common.Protocols;

namespace Post.Cmd.Api.Protocols;

public class NewPostResponse : ResponseBase
{
    public Guid Id { get; set; }
}
