using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;
internal sealed class PostRepository : IPostRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public PostRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(PostEntity post)
    {
        using var context = _contextFactory.CreateDbContext();
        await context.Posts.AddAsync(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        using var context = _contextFactory.CreateDbContext();

        var post = await GetPostByIdAsync(postId);

        if (post is null) return;

        context.Posts.Remove(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task<PostEntity?> GetPostByIdAsync(Guid postId)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(q => q.PostId.Equals(postId));
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<List<PostEntity>> GetPostsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(p => p.Comments)
            .AsNoTracking().ToListAsync();
    }

    public async Task<List<PostEntity>> GetPostsByAuthorAsync(string author)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(p => p.Comments).AsNoTracking()
            .Where(p => p.Author.Contains(author)).ToListAsync();
    }

    public async Task<List<PostEntity>> GetPostsWithCommentsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(p => p.Comments).AsNoTracking()
            .Where(p => p.Comments != null && p.Comments.Any()).ToListAsync();
    }

    public async Task<List<PostEntity>> GetPostsWithLikesAsync(int numberOfLikes)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(p => p.Comments).AsNoTracking()
            .Where(p => p.Likes > numberOfLikes).ToListAsync();
    }

    public async Task UpdateAsync(PostEntity post)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Posts.Update(post);
        _ = await context.SaveChangesAsync();
    }
}
