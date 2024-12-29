using CQRS.Core.Events;
using Post.Common.Events;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Post.Query.Infrastructure.Converters;
internal class EventJsonConverter : JsonConverter<EventBase>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(EventBase));
    }
    public override EventBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
        {
            throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
        }

        if (!doc.RootElement.TryGetProperty("Type", out var type))
        {
            throw new JsonException($" Could no detect the type discriminator property");
        }
        var typeDisciminitaor = type.GetString();
        var json = doc.RootElement.GetRawText();

        return typeDisciminitaor switch
        {
            nameof(PostCreatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            nameof(MessageUpdatedEvent) => JsonSerializer.Deserialize<MessageUpdatedEvent>(json, options),
            nameof(PostLikedEvent) => JsonSerializer.Deserialize<PostLikedEvent>(json, options),
            nameof(PostRemovedEvent) => JsonSerializer.Deserialize<PostRemovedEvent>(json, options),
            nameof(CommentAddedEvent) => JsonSerializer.Deserialize<CommentAddedEvent>(json, options),
            nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<CommentUpdatedEvent>(json, options),
            nameof(CommentRemovedEvent) => JsonSerializer.Deserialize<CommentRemovedEvent>(json, options),
            _=> throw new JsonException($"{nameof(typeDisciminitaor)} is not supported yet")
        };
    }

    public override void Write(Utf8JsonWriter writer, EventBase value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
