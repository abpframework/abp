using System;

namespace Volo.Abp.EventBus;

/// <summary>
/// Wraps arbitrary event data with a string-based event name for dynamic (type-less) event handling.
/// </summary>
public class DynamicEventData : IEventDataMayHaveTenantId
{
    public string EventName { get; }

    public object Data { get; }

    private Guid? _tenantId;
    private bool _hasTenantId;

    public DynamicEventData(string eventName, object data)
    {
        EventName = Check.NotNullOrWhiteSpace(eventName, nameof(eventName));
        Data = Check.NotNull(data, nameof(data));
    }

    /// <summary>
    /// <see cref="Data"/> is user data, so the tenant id can only come from the transport.
    /// </summary>
    public DynamicEventData SetTenantId(Guid? tenantId)
    {
        _tenantId = tenantId;
        _hasTenantId = true;
        return this;
    }

    public bool IsMultiTenant(out Guid? tenantId)
    {
        tenantId = _tenantId;
        return _hasTenantId;
    }
}
