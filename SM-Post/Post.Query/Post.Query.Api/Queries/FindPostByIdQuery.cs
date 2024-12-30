using CQRS.Core.Queries;

namespace Post.Query.Api.Queries;

public record FindPostByIdQuery : QueryBase
{
    public Guid Id { get; set; }
}
