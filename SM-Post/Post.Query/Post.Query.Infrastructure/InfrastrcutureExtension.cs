using CQRS.Core.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Dispatchers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;
using Post.Query.Infrastructure.Settings;

namespace Post.Query.Infrastructure;
public static class InfrastrcutureExtension
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .SetupSettings(configuration)
            .SetupDatabase(configuration);

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IEventHandler, Handlers.EventHandler>();
        services.AddScoped<IEventConsumer, Consumers.EventConsumer>();

        services.AddHostedService<Consumers.ConsumerHostedService>();
        return services;
    }

    private static IServiceCollection SetupSettings(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<ConsumerSettings>(configuration.GetSection(nameof(ConsumerSettings)));
        return services;
    }

    private static IServiceCollection SetupDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        Action<DbContextOptionsBuilder> optionsDbContext = (options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(configuration.GetConnectionString("cns"));
        });

        services.AddDbContext<DatabaseContext>(optionsDbContext);
        services.AddSingleton(new DatabaseContextFactory(optionsDbContext));

        var dataContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();

        dataContext.Database.EnsureCreated();
        return services;
    }

    private static IServiceCollection SetupHandlers(this IServiceCollection services)
    {

        return services;
    }
}
