using System;
using System.Threading.Tasks;
using Volo.Abp.MultiQueue.Options;

namespace Volo.Abp.MultiQueue.Subscriber;

public interface IQueueSubscriber
{
    /// <summary>
    /// 启动管道
    /// </summary>
    /// <returns></returns>
    void Start();

    /// <summary>
    /// 停止管道
    /// </summary>
    /// <returns></returns>
    void Stop();

    /// <summary>
    /// 订阅
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    Task SubscribeAsync(string topic, Type type);

    /// <summary>
    /// 订阅
    /// </summary>
    /// <typeparam name="TQueueResult"></typeparam>
    /// <param name="topic"></param>
    /// <returns></returns>
    Task SubscribeAsync<TQueueResult>(string topic) where TQueueResult : class, IQueueResult;

    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <param name="topic">Topic</param>
    /// <returns></returns>
    Task UnSubscribeAsync(string topic);
}

public interface IQueueSubscriber<TQueueOptions> : IQueueSubscriber where TQueueOptions : IQueueOptions
{

}
