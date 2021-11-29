using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;
using Polly.Extensions.Http;
using Volo.Abp.Cli.Auth;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.Licensing
{
    public class AbpIoApiKeyService : IApiKeyService, ITransientDependency
    {
        protected IJsonSerializer JsonSerializer { get; }
        protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        private readonly ILogger<AbpIoApiKeyService> _logger;
        private DeveloperApiKeyResult _apiKeyResult = null;
        private readonly CliHttpClientFactory _cliHttpClientFactory;

        public AbpIoApiKeyService(
            IJsonSerializer jsonSerializer,
            ICancellationTokenProvider cancellationTokenProvider,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            ILogger<AbpIoApiKeyService> logger, 
            CliHttpClientFactory cliHttpClientFactory)
        {
            JsonSerializer = jsonSerializer;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            _logger = logger;
            _cliHttpClientFactory = cliHttpClientFactory;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task<DeveloperApiKeyResult> GetApiKeyOrNullAsync(bool invalidateCache = false)
        {
            if (!AuthService.IsLoggedIn())
            {
                return null;
            }

            if (invalidateCache)
            {
                _apiKeyResult = null;
            }

            if (_apiKeyResult != null)
            {
                return _apiKeyResult;
            }

            var url = $"{CliUrls.WwwAbpIo}api/license/api-key";
            var client = _cliHttpClientFactory.CreateClient();

            using (var response = await client.GetHttpResponseMessageWithRetryAsync(url, CancellationTokenProvider.Token, _logger))
            {
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