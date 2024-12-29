using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
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

        services
            .AddOpenApi()
            .SetupCommandHandlers()
            .AddControllers();

        return services;
    }

    private static IServiceCollection SetupCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler, CommandHandler>();

        var commandHandler = services.BuildServiceProvider().GetService<ICommandHandler>();

        CommandDispatcher dispatcher = new();
        var pack = new ConventionPack { new GuidRepresentationConvention() };
        ConventionRegistry.Register("GuidRepresentationConvention", pack, t => true);
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

public sealed class GuidRepresentationConvention : IMemberMapConvention
{
    public string Name => "GuidRepresentationConvention";

    public void Apply(BsonMemberMap memberMap)
    {
        if (memberMap.MemberType == typeof(Guid) || memberMap.MemberType == typeof(Guid?))
        {
            memberMap.SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }
    }
}
