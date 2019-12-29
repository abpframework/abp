using Newtonsoft.Json;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;
using Polly.Extensions.Http;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.Licensing;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.NuGet
{
    public class NuGetService : ITransientDependency
    {
        public ILogger<VoloNugetPackagesVersionUpdater> Logger { get; set; }
        protected IJsonSerializer JsonSerializer { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }
        protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
        private readonly IApiKeyService _apiKeyService;

        public NuGetService(
            IJsonSerializer jsonSerializer,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            ICancellationTokenProvider cancellationTokenProvider, IApiKeyService apiKeyService)
        {
            JsonSerializer = jsonSerializer;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            CancellationTokenProvider = cancellationTokenProvider;
            _apiKeyService = apiKeyService;
            Logger = NullLogger<VoloNugetPackagesVersionUpdater>.Instance;
        }

        public async Task<SemanticVersion> GetLatestVersionOrNullAsync(string packageId, bool includePreviews = false, bool includeNightly = false)
        {
            var url = includeNightly ?
                $"https://www.myget.org/F/abp-nightly/api/v3/flatcontainer/{packageId.ToLowerInvariant()}/index.json" :
                $"https://api.nuget.org/v3-flatcontainer/{packageId.ToLowerInvariant()}/index.json";


            using (var client = new CliHttpClient(setBearerToken: false))
            {
                var responseMessage = await GetHttpResponseMessageWithRetryAsync(client, url);

                if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    var commercialNuGetUrl = await GetNuGetUrlForCommercialPackage(packageId);
                    responseMessage = await GetHttpResponseMessageWithRetryAsync(client, commercialNuGetUrl);
                }

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception($"ERROR: Remote server returns '{responseMessage.StatusCode}'");
                }

                await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(responseMessage);

                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                var versions = JsonSerializer
                    .Deserialize<NuGetVersionResultDto>(responseContent)
                    .Versions
                    .Select(SemanticVersion.Parse);

                if (!includePreviews && !includeNightly)
                {
                    versions = versions.Where(x => !x.IsPrerelease);
                }

                var semanticVersions = versions.ToList();
                return semanticVersions.Any() ? semanticVersions.Max() : null;
            }
        }

        private async Task<string> GetNuGetUrlForCommercialPackage(string packageId)
        {
            var apiKeyResult = await _apiKeyService.GetApiKeyOrNullAsync();
            return CliUrls.GetNuGetPackageInfoUrl(apiKeyResult.ApiKey, packageId);
        }

        private async Task<HttpResponseMessage> GetHttpResponseMessageWithRetryAsync(HttpClient client, string url)
        {
            return await HttpPolicyExtensions
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
                            Logger.LogWarning(
                                $"{retryCount}. HTTP request attempt failed to {url} with an error: HTTP {(int)responseMessage.Result.StatusCode}-{responseMessage.Exception.Message}. " +
                                $"Waiting {timeSpan.TotalSeconds} secs for the next try...");
                        }
                        else if (responseMessage.Result != null)
                        {
                            Logger.LogWarning(
                                $"{retryCount}. HTTP request attempt failed to {url} with an error: {(int)responseMessage.Result.StatusCode}-{responseMessage.Result.ReasonPhrase}. " +
                                $"Waiting {timeSpan.TotalSeconds} secs for the next try...");
                        }
                    })
                .ExecuteAsync(async () => await client.GetAsync(url, CancellationTokenProvider.Token));
        }


        public class NuGetVersionResultDto
        {
            [JsonProperty("versions")]
            public List<string> Versions { get; set; }
        }
    }
}
