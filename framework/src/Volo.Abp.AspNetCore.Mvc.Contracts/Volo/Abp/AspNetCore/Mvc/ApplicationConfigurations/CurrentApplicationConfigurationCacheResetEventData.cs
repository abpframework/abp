using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

/// <summary>
/// This event is used to invalidate current user's cached configuration.
/// </summary>
public class CurrentApplicationConfigurationCacheResetEventData
{
    public Guid? UserId { get; set; }

    public CurrentApplicationConfigurationCacheResetEventData()
    {

    }

    public CurrentApplicationConfigurationCacheResetEventData(Guid? userId)
    {
        UserId = userId;
    }
}
