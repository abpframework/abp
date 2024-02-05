using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Volo.Abp.Modularity;
using Volo.Abp.MultiQueue.Options;

namespace Volo.Abp.MultiQueue;

[DependsOn(typeof(AbpMultiQueueModule))]
public abstract class AbpMultiQueueModuleBase<TOptionsType> : AbpModule where TOptionsType : class, IQueueOptions
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var attr = typeof(TOptionsType).GetCustomAttribute<QueueOptionsTypeAttribute>();
        if (attr != null && !string.IsNullOrWhiteSpace(attr.Type))
        {
            var options = context.Services.ExecutePreConfiguredActions<QueueOptionsContainer>();
            var useOptions = options.Options.Where(p => p.Value.QueueType.ToLower() == attr.Type.ToLower()).ToArray();
            if (useOptions.Length > 0)
            {
                MethodInfo configureOptionsMethod = GetType().GetMethod(nameof(ConfigureOptions), BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (var item in useOptions)
                {
                    // 注册Options
                    var optionType = QueueOptionsExtension.GetOrCreateOptionType<TOptionsType>(item.Key);
                    object optionsVal = null;
                    if (item.Value.Options is IQueueOptions)
                    {
                        optionsVal = item.Value.Options;
                    }
                    else if (item.Value.Options is JToken jToken)
                    {
                        optionsVal = jToken.ToObject(optionType);
                    }

                    if (optionsVal != null)
                    {
                        configureOptionsMethod.MakeGenericMethod(optionType).Invoke(this, new object[] { optionsVal });
                    }

                    var publisherType = GetQueuePublisherType(optionType);
                    var subscriberType = GetQueueSubscriberType(optionType);

                    RegisterPublisher(context, publisherType.ServiceType, publisherType.ImplementationType);
                    RegisterSubscriber(context, subscriberType.ServiceType, subscriberType.ImplementationType);
                }
            }
        }
    }

    protected void ConfigureOptions<T>(IQueueOptions options) where T : class, TOptionsType
    {
        Configure<T>((opt) =>
        {
            SetOptionsVal(options, opt);
        });
    }

    protected void SetOptionsVal(object source, object target)
    {
        var sourceType = source.GetType();
        var targetType = target.GetType();


        if (sourceType.IsAssignableFrom(targetType))
        {
            var props = source.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.CanWrite)
                    prop.SetValue(target, prop.GetValue(source));
            }
        }
    }

    protected abstract (Type ServiceType, Type ImplementationType) GetQueuePublisherType(Type optionsType);

    protected abstract (Type ServiceType, Type ImplementationType) GetQueueSubscriberType(Type optionType);

    protected virtual void RegisterPublisher(ServiceConfigurationContext context, Type serviceType, Type implType)
    {
        context.Services.AddSingleton(serviceType, implType);
    }

    protected virtual void RegisterSubscriber(ServiceConfigurationContext context, Type serviceType, Type implType)
    {
        context.Services.AddSingleton(serviceType, implType);
    }
}
