using System;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EventBus.Distributed;

public class MyGenericDistributedEventHandler<TEvent> : IDistributedEventHandler<TEvent>
{
    private readonly ICurrentTenant _currentTenant;

    public MyGenericDistributedEventHandler(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public static Guid? TenantId { get; set; }

    public Task HandleEventAsync(TEvent eventData)
    {
        TenantId = _currentTenant.Id;
        return Task.CompletedTask;
    }
}
