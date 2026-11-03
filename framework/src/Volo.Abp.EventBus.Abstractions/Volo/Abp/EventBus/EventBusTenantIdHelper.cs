using System;

namespace Volo.Abp.EventBus;

public static class EventBusTenantIdHelper
{
    /// <summary>
    /// Throws for an unparsable value so the provider can fail the message
    /// instead of silently handling it in the host context.
    /// </summary>
    public static Guid? Parse(string? value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return null;
        }

        if (!Guid.TryParse(value, out var tenantId))
        {
            throw new AbpException(
                $"'{EventBusConsts.TenantIdHeaderName}' header of the distributed event was not a valid GUID: {value}");
        }

        return tenantId;
    }
}
