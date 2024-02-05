using System;
using System.Threading.Tasks;
using Volo.Abp.MultiQueue.Options;

namespace Volo.Abp.MultiQueue.Publisher;


public interface IQueuePublisher : IDisposable
{
    /// <summary>
    /// 发布
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task PublishAsync(string topic, object data);

    /// <summary>
    /// 批量发布
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task BatchPublishAsync(string topic, object[] data);
}

public interface IQueuePublisher<TQueueOptions> : IQueuePublisher where TQueueOptions : IQueueOptions
{

}