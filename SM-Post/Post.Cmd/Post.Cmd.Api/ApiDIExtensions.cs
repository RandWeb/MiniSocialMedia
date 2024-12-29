﻿using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using MongoDB.Bson.Serialization;
using Post.Cmd.Api.Commands;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Settings;
using Post.Common.Events;

namespace Post.Cmd.Api;

internal static class ApiDIExtensions
{
    public static IServiceCollection AddApiDI(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
        services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));
        services.AddOpenApi();
        return services;
    }

    private static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler, CommandHandler>();

        var commandHandler = services.BuildServiceProvider().GetService<ICommandHandler>();

        CommandDispatcher dispatcher = new();

        BsonClassMap.RegisterClassMap<EventBase>();
        BsonClassMap.RegisterClassMap<PostCreatedEvent>();
        BsonClassMap.RegisterClassMap<MessageUpdatedEvent>();
        BsonClassMap.RegisterClassMap<PostLikedEvent>();
        BsonClassMap.RegisterClassMap<PostRemovedEvent>();
        BsonClassMap.RegisterClassMap<CommentAddedEvent>();
        BsonClassMap.RegisterClassMap<CommentUpdatedEvent>();
        BsonClassMap.RegisterClassMap<CommentRemovedEvent>();

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
