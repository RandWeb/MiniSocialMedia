using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface ICommentRepository
{
    Task CreateAsync(CommentEntity comment);
    Task UpdateAsync(CommentEntity comment);
    Task RemoveAsync(Guid commentId);
    Task<CommentEntity> GetCommentByIdAsync(Guid commentId);
    Task<List<CommentEntity>> GetCommentsAsync();
    Task<List<CommentEntity>> GetCommentsByUserNameAsync(string username);
    Task<List<CommentEntity>> GetCommentsByPostIdAsync(Guid postId);
}
