using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;

namespace Post.Query.Infrastructure;
public static class InfrastrcutureExtension
{

    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, ConfigurationManager configuration)
    {
        Action<DbContextOptionsBuilder> optionsDbContext = (options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(configuration.GetConnectionString("cns"));
        });

        services.AddDbContext<DatabaseContext>(optionsDbContext);
        services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(optionsDbContext));

        var dataContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();

        dataContext.Database.EnsureCreated();

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IEventHandler, Handlers.EventHandler>();
        return services;
    }
}
