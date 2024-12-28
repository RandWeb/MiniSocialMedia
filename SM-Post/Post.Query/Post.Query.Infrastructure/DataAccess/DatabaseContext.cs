using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess;
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
/*    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostEntity>().HasKey(x => x.PostId);
        modelBuilder.Entity<CommentEntity>().HasKey(x => x.CommentId);
        modelBuilder.Entity<PostEntity>().HasMany(x => x.Comments).WithOne(x => x.Post).HasForeignKey(x => x.PostId);
    }*/
}
