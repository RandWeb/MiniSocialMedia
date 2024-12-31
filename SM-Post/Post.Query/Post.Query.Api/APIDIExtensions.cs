using CQRS.Core.Infrastructure;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Infrastructure.Dispatchers;

namespace Post.Query.Api;

public static class APIDIExtensions
{
    public static IServiceCollection AddApiDI(this IServiceCollection services)
    {
        
        services
            .SetupHandlers()
            .AddControllers();


        services.AddOpenApi();

        return services;
    }

    private static IServiceCollection SetupHandlers(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler, QueryHandler>();
        var queryHandler = services.BuildServiceProvider().GetService<IQueryHandler>();
        var discpacher = new QueryDispacher();

        discpacher.RegisterAsync<FindAllPostsQuery>(queryHandler.HandleAsync);
        discpacher.RegisterAsync<FindPostByIdQuery>(queryHandler.HandleAsync);
        discpacher.RegisterAsync<FindPostsByAuthorQuery>(queryHandler.HandleAsync);
        discpacher.RegisterAsync<FindPostsWithCommentsQuery>(queryHandler.HandleAsync);
        discpacher.RegisterAsync<FindPostsWithLikesQuery>(queryHandler.HandleAsync);

        services.AddSingleton<IQueryDispacher<PostEntity>>(discpacher);
        return services;
    }
}
