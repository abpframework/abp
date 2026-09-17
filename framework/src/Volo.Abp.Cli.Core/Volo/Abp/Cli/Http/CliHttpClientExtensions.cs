using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityModel.Client;
using Volo.Abp.Cli.Auth;
using Volo.Abp.Threading;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Cli.Http;

public static class CliHttpClientExtensions
{
    public static void AddAbpAuthenticationToken(this HttpClient httpClient)
    {
        if (!AuthService.IsLoggedIn())
        {
            return;
        }

        var accessToken = File.ReadAllText(CliPaths.AccessToken, Encoding.UTF8);
        if (!accessToken.IsNullOrEmpty())
        {
            httpClient.SetBearerToken(accessToken);
        }
    }

    public static async Task<HttpResponseMessage> GetHttpResponseMessageWithRetryAsync<T>
    (
        this HttpClient httpClient,
        string url,
        CancellationToken? cancellationToken = null,
        ILogger<T> logger = null,
        IEnumerable<TimeSpan> sleepDurations = null
    )
    {
        if (sleepDurations == null)
        {
            sleepDurations = new[]
            {
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4),
                    TimeSpan.FromSeconds(7)
                };
        }

        if (cancellationToken == null)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(httpClient.Timeout);
            cancellationToken = cancellationTokenSource.Token;
        }

        using var delayEnumerator = sleepDurations.GetEnumerator();

        return await RetryHelper.ExecuteAsync(
            _ => httpClient.GetAsync(url, cancellationToken.Value),
            new RetryOptions<HttpResponseMessage>
            {
                MaxRetryCount = int.MaxValue,
                DelayFactory = _ => delayEnumerator.Current,
                ShouldRetryOnException = exception =>
                    exception is HttpRequestException && delayEnumerator.MoveNext(),
                ShouldRetryOnResult = response =>
                    !response.IsSuccessStatusCode && delayEnumerator.MoveNext(),
                OnRetry = attempt =>
                {
                    if (attempt.Exception != null)
                    {
                        logger?.LogWarning(
                            $"{attempt.RetryCount}. HTTP request attempt failed to {url} with an error: {attempt.Exception.Message}. " +
                            $"Waiting {attempt.RetryDelay.TotalSeconds} secs for the next try...");
                    }
                    else if (attempt.Result != null)
                    {
                        logger?.LogWarning(
                            $"{attempt.RetryCount}. HTTP request attempt failed to {url} with an error: {(int)attempt.Result.StatusCode}-{attempt.Result.ReasonPhrase}. " +
                            $"Waiting {attempt.RetryDelay.TotalSeconds} secs for the next try...");
                    }
                    return Task.CompletedTask;
                }
            });
    }
}
