namespace Post.Query.Infrastructure.Settings;

public class ConsumerSettings
{
    public string GroupId { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableAutoCommit { get; set; }
    public string AutoOffsetReset { get; set; }
    public bool AllowAutoCreateTopics { get; set; }
}


