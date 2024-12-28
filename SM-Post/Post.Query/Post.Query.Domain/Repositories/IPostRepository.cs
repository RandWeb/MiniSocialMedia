using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;
public interface IPostRepository
{
    Task CreateAsync(PostEntity post);
    Task DeleteAsync(Guid postId);
    Task UpdateAsync(PostEntity post);

    Task<PostEntity> GetPostByIdAsync(Guid postId);

    Task<List<PostEntity>> GetPostsAsync();

    Task<List<PostEntity>> GetPostsByAuthorAsync(string author);

    Task<List<PostEntity>> GetPostsWithLikesAsync(int numberOfLikes);

    Task<List<PostEntity>> GetPostsWithCommentsAsync();
}
