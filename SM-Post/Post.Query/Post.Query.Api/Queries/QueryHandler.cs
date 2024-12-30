using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

internal sealed class QueryHandler(IPostRepository postRepository) : IQueryHandler
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
    {
        return await _postRepository.GetPostsAsync();
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
    {
        return [await _postRepository.GetPostByIdAsync(query.Id)];
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query)
    {
        return await _postRepository.GetPostsByAuthorAsync(query.Author);
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
    {
        return await _postRepository.GetPostsWithCommentsAsync();
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
    {
        return await _postRepository.GetPostsWithLikesAsync(query.NumberOfLikes);
    }
}
