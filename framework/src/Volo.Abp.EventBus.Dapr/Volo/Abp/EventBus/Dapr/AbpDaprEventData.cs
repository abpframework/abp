using System;
using System.Reflection;

namespace Volo.Abp.EventBus.Dapr;

public class AbpDaprEventData<TData>
{
    public TData Data { get; set; }

    public string CorrelationId { get; set; }

    public AbpDaprEventData(TData data, string correlationId)
    {
        Data = data;
        CorrelationId = correlationId;
    }

    public static object Create(object data, string correlationId)
    {
        return Activator.CreateInstance(
            typeof(AbpDaprEventData<>).MakeGenericType(data.GetType()),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            new object[] { data, correlationId },
            culture: null)!;
    }
}
