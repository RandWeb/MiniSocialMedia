using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

internal sealed class CommentRepository : ICommentRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public CommentRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(CommentEntity comment)
    {
        using var context = _contextFactory.CreateDbContext();
        await context.Comments.AddAsync(comment);
        _ = await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid commentId)
    {
        using var context = _contextFactory.CreateDbContext();
        var comment = await GetCommentByIdAsync(commentId);

        if (comment is null) return;

        context.Comments.Remove(comment);
        _ = await context.SaveChangesAsync();
    }

    public async Task<CommentEntity> GetCommentByIdAsync(Guid commentId)
    {
        using var context = _contextFactory.CreateDbContext();

        return await context.Comments.FindAsync(commentId);
    }

    public async Task<List<CommentEntity>> GetCommentsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Comments.ToListAsync();
    }

    public async Task<List<CommentEntity>> GetCommentsByUserNameAsync(string username)
    {
        using var context = _contextFactory.CreateDbContext();

        return await context.Comments.AsNoTracking()
            .Where(c => c.UserName == username).ToListAsync();
    }

    public async Task<List<CommentEntity>> GetCommentsByPostIdAsync(Guid postId)
    {
        using var context = _contextFactory.CreateDbContext();

        return await context.Comments.AsNoTracking()
            .Where(c => c.PostId == postId).ToListAsync();
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Comments.Update(comment);
        _ = await context.SaveChangesAsync();
    }
}