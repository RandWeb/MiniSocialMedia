using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Post.Query.Infrastructure.Consumers;
internal sealed class ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider) : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer started");

        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
            Task.Run(() => eventConsumer.Consume(topic), cancellationToken);
        };

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer stopped");

        await Task.CompletedTask;
    }
}
