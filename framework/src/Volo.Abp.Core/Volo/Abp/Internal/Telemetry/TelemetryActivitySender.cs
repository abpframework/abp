using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;

namespace Volo.Abp.Internal.Telemetry;

public class TelemetryActivitySender : ITelemetryActivitySender, IScopedDependency
{
    private readonly ITelemetryActivityStorage _telemetryActivityStorage;
    private readonly ILogger<TelemetryActivitySender> _logger;

    private const int ActivitySendBatchSize = 50;
    private const int MaxRetryAttempts = 3;
    private const int RetryDelayMilliseconds = 1000;

    public TelemetryActivitySender(ITelemetryActivityStorage telemetryActivityStorage, ILogger<TelemetryActivitySender> logger)
    {
        _telemetryActivityStorage = telemetryActivityStorage;
        _logger = logger;
    }

    public async Task TrySendQueuedActivitiesAsync()
    {
        if (!_telemetryActivityStorage.ShouldSendActivities())
        {
            return;
        }

        await SendActivitiesAsync();
    }


    private async Task SendActivitiesAsync()
    {
        try
        {
            var activities = _telemetryActivityStorage.GetActivities();
            var batches = CreateActivityBatches(activities);

            using var httpClient = new HttpClient();
            ConfigureHttpClientAuthentication(httpClient);

            foreach (var batch in batches)
            {
                var isSuccessful = await TrySendBatchWithRetriesAsync(httpClient, batch);
                
                if (!isSuccessful)
                {
                    break;
                }
            }
        }
        catch
        {
            //ignored
        }
    }


    private async Task<bool> TrySendBatchWithRetriesAsync(HttpClient httpClient, ActivityEvent[] activities)
    {
        var currentAttempt = 0;

        while (currentAttempt < MaxRetryAttempts)
        {
            try
            {
                var response = await httpClient.PostAsync($"{AbpPlatformUrls.AbpTelemetryApiUrl}api/telemetry/collect", new StringContent(JsonSerializer.Serialize(activities), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    _telemetryActivityStorage.DeleteActivities(activities);
                }
                else
                {
                    _logger.LogWithLevel(LogLevel.Trace,
                        $"Failed to send telemetry activities. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                    _telemetryActivityStorage.MarkActivitiesAsFailed(activities);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWithLevel(LogLevel.Trace, $"Error sending telemetry activities: {ex.Message}");
                currentAttempt++;
                await Task.Delay(currentAttempt * RetryDelayMilliseconds);
            }
        }

        _logger.LogWithLevel(LogLevel.Trace, "Max retries reached. Failed to send telemetry activities.");

        return false;
    }

    private static IEnumerable<ActivityEvent[]> CreateActivityBatches(List<ActivityEvent> activities)
    {
        return activities
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / ActivitySendBatchSize)
            .Select(x => x.Select(v => v.Value).ToArray());
    }

    private static void ConfigureHttpClientAuthentication(HttpClient httpClient)
    {
        if (!File.Exists(TelemetryPaths.AccessToken))
        {
            return;
        }

        var accessToken = File.ReadAllText(TelemetryPaths.AccessToken, Encoding.UTF8);

        if (accessToken.IsNullOrEmpty())
        {
            return;
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
}