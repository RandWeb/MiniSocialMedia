using Post.Common.Protocols;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Protocols;

public record PostLookupResponse : ResponseBase
{
    public List<PostEntity> Posts { get; set; }
}
