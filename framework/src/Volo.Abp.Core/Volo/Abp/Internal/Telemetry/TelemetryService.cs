using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;
using ActivityContext = Volo.Abp.Internal.Telemetry.Activity.ActivityContext;

namespace Volo.Abp.Internal.Telemetry;

public class TelemetryService : ITelemetryService, IScopedDependency
{
    private readonly ITelemetryActivitySender _telemetryActivitySender;
    private readonly ITelemetryActivityEventBuilder _telemetryActivityEventBuilder;
    private readonly ITelemetryActivityStorage _telemetryActivityStorage;

    public TelemetryService(ITelemetryActivitySender telemetryActivitySender,
        ITelemetryActivityEventBuilder telemetryActivityEventBuilder,
        ITelemetryActivityStorage telemetryActivityStorage)
    {
        _telemetryActivitySender = telemetryActivitySender;
        _telemetryActivityEventBuilder = telemetryActivityEventBuilder;
        _telemetryActivityStorage = telemetryActivityStorage;
    }


    public IAsyncDisposable TrackActivityAsync(string activityName,
        Action<Dictionary<string, object>>? additionalProperties = null)
    {
        Check.NotNullOrEmpty(activityName, nameof(activityName));
        var stopwatch = Stopwatch.StartNew();
        var context = ActivityContext.Create(activityName, additionalProperties: additionalProperties);

        return new AsyncDisposeFunc(async () =>
        {
            stopwatch.Stop();
            context.Current[ActivityPropertyNames.ActivityDuration] = stopwatch.ElapsedMilliseconds;
            await AddActivityAsync(context);
        });
    }

    public async Task AddActivityAsync(string activityName,
        Action<Dictionary<string, object>>? additionalProperties = null)
    {
        Check.NotNullOrEmpty(activityName, nameof(activityName));
        var context = ActivityContext.Create(activityName, additionalProperties: additionalProperties);
        await AddActivityAsync(context);
    }

    public async Task AddErrorActivityAsync(Action<Dictionary<string, object>> additionalProperties)
    {
        var context = ActivityContext.Create(ActivityNameConsts.Error, additionalProperties: additionalProperties);
        await AddActivityAsync(context);
    }

    public async Task AddErrorActivityAsync(string errorMessage)
    {
        var context = ActivityContext.Create(ActivityNameConsts.Error, errorMessage);
        await AddActivityAsync(context);
    }

    public async Task AddErrorForActivityAsync(string failingActivity, string errorMessage)
    {
        Check.NotNullOrEmpty(failingActivity, nameof(failingActivity));
        var context = ActivityContext.Create(ActivityNameConsts.Error, errorMessage, configure =>
        {
            configure[ActivityPropertyNames.FailingActivity] = failingActivity;
        });
        await AddActivityAsync(context);
    }

    private Task AddActivityAsync(ActivityContext context)
    {
        _ = Task.Run(async () =>
        {
            await BuildAndSendActivityAsync(context);
        });

        return Task.CompletedTask;
    }

    private async Task BuildAndSendActivityAsync(ActivityContext context)
    {
        try
        {
            var activityEvent = await _telemetryActivityEventBuilder.BuildAsync(context);
            if (activityEvent is null)
            {
                return;
            }

            _telemetryActivityStorage.SaveActivity(activityEvent);
            await _telemetryActivitySender.TrySendQueuedActivitiesAsync();
        }
        catch
        {
            //ignored
        }
    }
}