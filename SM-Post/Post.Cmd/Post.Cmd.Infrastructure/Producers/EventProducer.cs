using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;
using Post.Cmd.Infrastructure.Settings;
using System.Text.Json;

namespace Post.Cmd.Infrastructure.Producers;
internal class EventProducer : IEventProducer
{
    private readonly KafkaSettings _kafkaSettings;
    public EventProducer(IOptions<KafkaSettings> options)
    {
        _kafkaSettings = options.Value;
    }
    public async Task ProduceAsync<T>(string topic, T @event) where T : EventBase
    {
        using var producer = new ProducerBuilder<string, string>(new ProducerConfig
        {
            BootstrapServers = $"{_kafkaSettings.Host}:{_kafkaSettings.Port}",
        }).SetKeySerializer(Serializers.Utf8)
        .SetValueSerializer(Serializers.Utf8).Build();

        var eventMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event, @event.GetType()),
        };

        var deliverResult = await producer.ProduceAsync(topic, eventMessage);

        if (deliverResult.Status is PersistenceStatus.NotPersisted)
        {
            throw new Exception($"Could not produce {@event.GetType().Name} message to topic - {topic} due to folloing reason: {deliverResult.Message}");
        }
    }
}
