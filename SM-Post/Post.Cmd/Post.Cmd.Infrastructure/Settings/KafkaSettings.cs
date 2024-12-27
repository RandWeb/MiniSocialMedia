namespace Post.Cmd.Infrastructure.Settings;
public class KafkaSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string GroupId { get; set; }
    public string Topic { get; set; }
}
