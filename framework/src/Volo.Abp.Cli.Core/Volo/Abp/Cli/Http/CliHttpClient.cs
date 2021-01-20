using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Polly;
using Polly.Extensions.Http;
using Volo.Abp.Cli.Auth;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Cli.Http
{
    public class CliHttpClient : HttpClient
    {
        public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(1);

        public CliHttpClient(TimeSpan? timeout = null)
            : base(new CliHttpClientHandler())
        {
            Timeout = timeout ?? DefaultTimeout;

            AddAuthentication(this);
        }

        public CliHttpClient(bool setBearerToken) : base(new CliHttpClientHandler())
        {
            Timeout = DefaultTimeout;

            if (setBearerToken)
            {
                AddAuthentication(this);
            }
        }

        private static void AddAuthentication(HttpClient client)
        {
            if (!AuthService.IsLoggedIn())
            {
                return;
            }

            var accessToken = File.ReadAllText(CliPaths.AccessToken, Encoding.UTF8);
            if (!accessToken.IsNullOrEmpty())
            {
                client.SetBearerToken(accessToken);
            }
        }

        public async Task<HttpResponseMessage> GetHttpResponseMessageWithRetryAsync<T>
        (
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

            cancellationToken ??= CancellationToken.None;

            return await HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => !msg.IsSuccessStatusCode)
                .WaitAndRetryAsync(sleepDurations,
                    (responseMessage, timeSpan, retryCount, context) =>
                    {
                        if (responseMessage.Exception != null)
                        {
                            string httpErrorCode = responseMessage.Result == null ?
                                httpErrorCode = string.Empty :
                                "HTTP-" + (int)responseMessage.Result.StatusCode + ", ";

                            logger?.LogWarning(
                                $"{retryCount}. HTTP request attempt failed to {url} with an error: {httpErrorCode}{responseMessage.Exception.Message}. " +
                                $"Waiting {timeSpan.TotalSeconds} secs for the next try...");
                        }
                        else if (responseMessage.Result != null)
                        {
                            logger?.LogWarning(
                                $"{retryCount}. HTTP request attempt failed to {url} with an error: {(int)responseMessage.Result.StatusCode}-{responseMessage.Result.ReasonPhrase}. " +
                                $"Waiting {timeSpan.TotalSeconds} secs for the next try...");
                        }
                    })
                .ExecuteAsync(async () => await this.GetAsync(url, cancellationToken.Value));
        }

    }
}
