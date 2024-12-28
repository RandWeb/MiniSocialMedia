

using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess;

public class DatabaseContextFactory
{
    private readonly Action<DbContextOptionsBuilder> optionsBuilder;

    public DatabaseContextFactory(Action<DbContextOptionsBuilder> optionsBuilder)
    {
        this.optionsBuilder = optionsBuilder;
    }

    public DatabaseContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder(options);

        return new DatabaseContext(options.Options);
    }

}