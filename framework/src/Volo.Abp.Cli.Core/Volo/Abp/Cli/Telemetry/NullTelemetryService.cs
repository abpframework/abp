using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Internal.Telemetry;
using Volo.Abp.Internal.Telemetry.Activity;

namespace Volo.Abp.Cli.Telemetry;

public class NullTelemetryService : ITelemetryService
{
    public IAsyncDisposable TrackActivity(ActivityEvent activityData)
    {
        return NullAsyncDisposable.Instance;
    }
    public IAsyncDisposable TrackActivityAsync(string activityName, Action<Dictionary<string, object>>? additionalProperties = null)
    {
        return NullAsyncDisposable.Instance;
    }

    public Task AddActivityAsync(string activityName, Action<Dictionary<string, object>>? additionalProperties = null)
    {
        return Task.CompletedTask;
    }

    public Task AddErrorActivityAsync(Action<Dictionary<string, object>> additionalProperties)
    {
        return Task.CompletedTask;
    }

    public Task AddErrorActivityAsync(string errorMessage)
    {
        return Task.CompletedTask;
    }

    public Task AddErrorForActivityAsync(string failingActivity, string errorMessage)
    {
        return Task.CompletedTask;
    }
}