using System;
using Newtonsoft.Json;
using System.Text;

namespace Volo.Abp.MultiQueue.Subscriber;

public interface IQueueResult
{
    public string Source { get; set; }

    public DateTime Time { get; set; }
}

public abstract class QueueResult : IQueueResult
{
    public string Source { get; set; }
    public DateTime Time { get; set; }
}

public abstract class QueueResult<TObj> : QueueResult
{
    public TObj Data { get; private set; }

    protected void SetData(byte[] data)
    {
        Data = DeData(data);
    }

    protected abstract TObj DeData(byte[] data);
}

/// <summary>
/// 原始返回值
/// </summary>
public class OriginalEventData : QueueResult<byte[]>
{
    protected override byte[] DeData(byte[] data)
    {
        return data;
    }
}

/// <summary>
/// JSON返回值
/// </summary>
/// <typeparam name="TObj"></typeparam>
public class JsonQueueResult<TObj> : QueueResult<TObj>
{
    protected override TObj DeData(byte[] data)
    {
        return JsonConvert.DeserializeObject<TObj>(Encoding.UTF8.GetString(data));
    }
}
