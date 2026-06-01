using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Internal.Telemetry;
public interface ITelemetryService
{
    IAsyncDisposable TrackActivityAsync(string activityName, Action<Dictionary<string,object>>? additionalProperties = null);
    Task AddActivityAsync(string activityName, Action<Dictionary<string,object>>? additionalProperties = null);
    Task AddErrorActivityAsync(Action<Dictionary<string, object>> additionalProperties);
    Task AddErrorActivityAsync(string errorMessage);
    Task AddErrorForActivityAsync(string failingActivity, string errorMessage);
}