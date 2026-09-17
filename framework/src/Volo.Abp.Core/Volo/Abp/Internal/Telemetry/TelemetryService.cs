using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;
using ActivityContext = Volo.Abp.Internal.Telemetry.Activity.ActivityContext;

namespace Volo.Abp.Internal.Telemetry;

public class TelemetryService : ITelemetryService, IScopedDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TelemetryService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
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
        var telemetryTask = Task.Run(async () =>
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var telemetryActivityEventBuilder = scope.ServiceProvider.GetRequiredService<ITelemetryActivityEventBuilder>();
            var telemetryActivityStorage = scope.ServiceProvider.GetRequiredService<ITelemetryActivityStorage>();
            var telemetryActivitySender = scope.ServiceProvider.GetRequiredService<ITelemetryActivitySender>();

            await BuildAndSendActivityAsync(context,
                telemetryActivityEventBuilder,
                telemetryActivityStorage,
                telemetryActivitySender);
        });

        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            try
            {
                telemetryTask.Wait(TimeSpan.FromSeconds(10));
            }
            catch
            {
                // ignored
            }
        };
        
        Console.CancelKeyPress += (_, _) =>
        {
            try
            {
                telemetryTask.Wait(TimeSpan.FromSeconds(10));
            }
            catch
            {
                // ignored
            }
        };

        return Task.CompletedTask;
    }

    private static async Task BuildAndSendActivityAsync(
        ActivityContext context,
        ITelemetryActivityEventBuilder telemetryActivityEventBuilder,
        ITelemetryActivityStorage telemetryActivityStorage,
        ITelemetryActivitySender telemetryActivitySender)
    {
        try
        {
            var activityEvent = await telemetryActivityEventBuilder.BuildAsync(context);
            if (activityEvent is null)
            {
                return;
            }

            telemetryActivityStorage.SaveActivity(activityEvent);
            await telemetryActivitySender.TrySendQueuedActivitiesAsync();
        }
        catch
        {
            //ignored
        }
    }
}