using System.Collections.Generic;

namespace Volo.Abp.MultiQueue.Options;
public class QueueOptionsContainer
{
    public QueueOptionsContainer()
    {
        Options = new Dictionary<string, QueueOptionsWarp>();
    }

    public Dictionary<string, QueueOptionsWarp> Options { get; set; }

    public void AddOptions(QueueOptionsWarp options)
    {
        if (!Options.ContainsKey(options.Key))
        {
            Options.Add(options.Key, options);
        }
    }
}
