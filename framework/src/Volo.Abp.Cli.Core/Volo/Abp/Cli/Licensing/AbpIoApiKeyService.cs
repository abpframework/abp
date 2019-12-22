using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;
using Polly.Extensions.Http;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.Licensing
{
    public class AbpIoApiKeyService : IApiKeyService, ITransientDependency
    {
        protected IJsonSerializer JsonSerializer { get; }
        protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
        private readonly ILogger<AbpIoApiKeyService> _logger;

        public AbpIoApiKeyService(IJsonSerializer jsonSerializer, IRemoteServiceExceptionHandler remoteServiceExceptionHandler, ILogger<AbpIoApiKeyService> logger)
        {
            JsonSerializer = jsonSerializer;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            _logger = logger;
        }

        public async Task<DeveloperApiKeyResult> GetApiKeyOrNullAsync()
        {
            using (var client = new CliHttpClient())
            {
                var response = await HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => !msg.IsSuccessStatusCode)
                    .WaitAndRetryAsync(new[]
                        {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(7)
                        },
                        (responseMessage, timeSpan, retryCount, context) =>
                        {
                            if (responseMessage.Exception != null)
                            {
                                _logger.LogWarning(
                                    $"{retryCount}. request attempt failed with an error: \"{responseMessage.Exception.Message}\". " +
                                    $"Waiting {timeSpan.TotalSeconds} secs for the next try...");
                            }
                            else if (responseMessage.Result != null)
                            {
                                _logger.LogWarning(
                                    $"{retryCount}. request attempt failed with {responseMessage.Result.StatusCode}-{responseMessage.Result.ReasonPhrase}. " +
                                    $"Waiting {timeSpan.TotalSeconds} secs for the next try...");
                            }
                        })
                    .ExecuteAsync(async () => await client.GetAsync($"{CliUrls.WwwAbpIo}api/license/api-key"));

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"ERROR: Remote server returns '{response.StatusCode}'");
                }

                await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DeveloperApiKeyResult>(responseContent);
            }
        }
    }
}