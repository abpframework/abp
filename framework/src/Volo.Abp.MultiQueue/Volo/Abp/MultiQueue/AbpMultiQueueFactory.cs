using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiQueue.Options;
using Volo.Abp.MultiQueue.Publisher;
using Volo.Abp.MultiQueue.Subscriber;

namespace Volo.Abp.MultiQueue;

public interface IAbpMultiQueueFactory : ISingletonDependency
{
    /// <summary>
    /// 获取Queue发布者
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IQueuePublisher GetPublisher(string key);

    /// <summary>
    /// 获取Queue订阅者
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IQueueSubscriber GetSubscriber(string key);
}

public class AbpMultiQueueFactory : IAbpMultiQueueFactory
{
    protected IServiceProvider ServiceProvider { get; }

    public AbpMultiQueueFactory(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public IQueuePublisher GetPublisher(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException("Key is not null or empty.");

        var optionType = QueueOptionsExtension.GetOptionType(key);
        if (optionType == null) return null;

        var type = typeof(IQueuePublisher<>).MakeGenericType(optionType);
        var publisher = ServiceProvider.GetRequiredService(type) as IQueuePublisher;
        return publisher;
    }

    public IQueueSubscriber GetSubscriber(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException("Key is not null or empty.");

        var optionType = QueueOptionsExtension.GetOptionType(key);
        if (optionType == null) return null;

        var type = typeof(IQueueSubscriber<>).MakeGenericType(optionType);
        var subscriber = ServiceProvider.GetRequiredService(type) as IQueueSubscriber;
        return subscriber;
    }
}
