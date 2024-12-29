using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Settings;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Post.Query.Infrastructure.Consumers;
internal sealed class EventConsumer(
    IOptions<ConsumerSettings> options,
    IEventHandler eventHandler
    ) : IEventConsumer
{
    private readonly ConsumerSettings _consumerSettings = options.Value;
    private readonly IEventHandler _eventHandler = eventHandler;

    public async void Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(new ConsumerConfig
        {
            GroupId = _consumerSettings.GroupId,
            BootstrapServers = $"{_consumerSettings.Host}:{_consumerSettings.Port}",
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(_consumerSettings.AutoOffsetReset),
            EnableAutoCommit = _consumerSettings.EnableAutoCommit,
            AllowAutoCreateTopics = _consumerSettings.AllowAutoCreateTopics
        })
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();
            if (consumeResult?.Message is null) continue;
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                Converters = { new EventJsonConverter() },
                PropertyNameCaseInsensitive = true
            };
            JsonSerializerOptions options = jsonSerializerOptions;
            var @event = JsonSerializer.Deserialize<EventBase>(consumeResult.Message.Value, options);

            var handlerMethod = _eventHandler.GetType().GetMethod("On", [@event.GetType()]);

            ArgumentNullException.ThrowIfNull(handlerMethod, $"Handler is not found");

            handlerMethod.Invoke(_eventHandler, [@event]);
            consumer.Commit(consumeResult);
        }
    }
}
