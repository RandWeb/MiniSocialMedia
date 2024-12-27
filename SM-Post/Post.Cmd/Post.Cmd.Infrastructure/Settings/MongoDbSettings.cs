using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Settings;
public class MongoDbSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}
