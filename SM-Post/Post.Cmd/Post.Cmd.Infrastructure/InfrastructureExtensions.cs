using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Microsoft.Extensions.DependencyInjection;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Producers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

namespace Post.Cmd.Infrastructure;
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
    {
        services.AddScoped<IEventStoreRepository, EventStoreRepository>();
        services.AddScoped<IEventStore, EventStore>();
        services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
        services.AddScoped<IEventProducer, EventProducer>();
        return services;
    }
}
