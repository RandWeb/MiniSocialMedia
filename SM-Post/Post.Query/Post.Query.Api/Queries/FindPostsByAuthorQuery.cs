using CQRS.Core.Queries;

namespace Post.Query.Api.Queries;

public record FindPostsByAuthorQuery : QueryBase
{
    public string Author { get; set; }
}
