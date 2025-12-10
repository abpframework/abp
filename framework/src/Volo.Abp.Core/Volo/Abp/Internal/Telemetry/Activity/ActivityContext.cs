using System;
using System.Collections.Generic;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Constants.Enums;

namespace Volo.Abp.Internal.Telemetry.Activity;

public class ActivityContext
{
    public ActivityEvent Current { get; }
    public Dictionary<string, object> ExtraProperties { get; } = new();
    public bool IsTerminated { get; private set; }
    
    public Guid? ProjectId => Current.Get<Guid?>(ActivityPropertyNames.ProjectId);

    public Guid? SolutionId => Current.Get<Guid?>(ActivityPropertyNames.SolutionId);

    public SessionType? SessionType => Current.Get<SessionType?>(ActivityPropertyNames.SessionType);

    public string? DeviceId => Current.Get<string?>(ActivityPropertyNames.DeviceId);
    
    public string? SolutionPath => TryGetValue(ActivityPropertyNames.SolutionPath, out var solutionPath)
        ? solutionPath?.ToString()
        : null;

    private ActivityContext(ActivityEvent current)
    {
        Current = current;
    }
    
    public static ActivityContext Create(string activityName, string? details = null,
        Action<Dictionary<string, object>>? additionalProperties = null)
    {
        var activity = new ActivityEvent(activityName, details);
        
        if (additionalProperties is not null)
        {
            var additionalPropertiesDict = new Dictionary<string, object>();
            activity[ActivityPropertyNames.AdditionalProperties] = additionalPropertiesDict;
            additionalProperties.Invoke(additionalPropertiesDict);
        }

        return new ActivityContext(activity);
    }

    public bool TryGetValue(string key, out object? value)
    {
        if (Current.TryGetValue(key, out value))
        {
            return true;
        }
        
        if (ExtraProperties.TryGetValue(key, out value))
        {
            return true;
        }
        
        if (Current.TryGetValue(ActivityPropertyNames.AdditionalProperties, out var additionalProperties) &&
            additionalProperties is Dictionary<string, object> additionalPropertiesDict &&
            additionalPropertiesDict.TryGetValue(key, out value))
        {
            return true;
        }

        return false;
    }
    
    public void Terminate()
    {
        IsTerminated = true;
    }
}