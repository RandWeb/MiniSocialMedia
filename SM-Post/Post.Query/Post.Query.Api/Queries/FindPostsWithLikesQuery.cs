using CQRS.Core.Queries;

namespace Post.Query.Api.Queries;

public record FindPostsWithLikesQuery : QueryBase
{
    public int NumberOfLikes { get; set; }
}
