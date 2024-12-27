using CQRS.Core.Infrastructure;
using Post.Cmd.Api.Commands;
using Post.Cmd.Infrastructure.Dispatchers;

namespace Post.Cmd.Api;

internal static class ApiDIExtensions
{
    public static IServiceCollection AddApiDI(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }

    private static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler, CommandHandler>();

        var commandHandler = services.BuildServiceProvider().GetService<ICommandHandler>();

        CommandDispatcher dispatcher = new();

        dispatcher.RegisterHandler<AddPostCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<RemovePostCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateMessageCommand>(commandHandler.HandleAsync);

        dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateCommentCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);

        services.AddSingleton<ICommandDispatcher>(_ => dispatcher);
        return services;
    }
}
