namespace Volo.Abp.MultiQueue.Options;

public class QueueOptionsWarp
{
    /// <summary>
    /// 唯一键
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 管道类型
    /// </summary>
    public string QueueType { get; set; }

    /// <summary>
    /// 配置
    /// </summary>
    public object Options { get; set; }
}