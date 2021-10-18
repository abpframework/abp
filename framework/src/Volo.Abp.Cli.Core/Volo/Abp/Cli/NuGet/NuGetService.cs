using System;
using Newtonsoft.Json;
using NuGet.Versioning;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        private readonly CliHttpClientFactory _cliHttpClientFactory;
        private DeveloperApiKeyResult _apiKeyResult;

        public NuGetService(
            IJsonSerializer jsonSerializer,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            ICancellationTokenProvider cancellationTokenProvider,
            IApiKeyService apiKeyService,
            CliHttpClientFactory cliHttpClientFactory)
        {
            JsonSerializer = jsonSerializer;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            CancellationTokenProvider = cancellationTokenProvider;
            _apiKeyService = apiKeyService;
            _cliHttpClientFactory = cliHttpClientFactory;
            Logger = NullLogger<VoloNugetPackagesVersionUpdater>.Instance;
        }

        public async Task<SemanticVersion> GetLatestVersionOrNullAsync(string packageId, bool includeNightly = false, bool includeReleaseCandidates = false)
        {
            var versionList = await GetPackageVersionListAsync(packageId, includeNightly, includeReleaseCandidates);

            if (versionList == null)
            {
                return null;
            }

            List<SemanticVersion> versions;

            if (!includeNightly && !includeReleaseCandidates)
            {
                versions = versionList
                .Select(SemanticVersion.Parse)
                .OrderByDescending(v => v, new VersionComparer()).ToList();

                versions = versions.Where(x => !x.IsPrerelease).ToList();
            }
            else if (!includeNightly && includeReleaseCandidates)
            {
                versions = versionList
                    .Where(v => !v.Contains("-preview"))
                    .Select(SemanticVersion.Parse)
                    .OrderByDescending(v => v, new VersionComparer()).ToList();
            }
            else
            {
                versions = versionList
                    .Select(SemanticVersion.Parse)
                    .OrderByDescending(v => v, new VersionComparer()).ToList();
            }

            return versions.Any() ? versions.Max() : null;

        }

        public async Task<List<string>> GetPackageVersionListAsync(string packageId, bool includeNightly = false,
            bool includeReleaseCandidates = false)
        {
            if (includeNightly)
            {
                var includeNightlyUrl = $"https://www.myget.org/F/abp-nightly/api/v3/flatcontainer/{packageId.ToLowerInvariant()}/index.json";
                return await GetPackageVersionListFromUrlAsync(includeNightlyUrl);
            }

            var nugetUrl = $"https://api.nuget.org/v3-flatcontainer/{packageId.ToLowerInvariant()}/index.json";
            var commercialUrl = await GetNuGetUrlForCommercialPackage(packageId);

            return (await GetPackageVersionListFromUrlAsync(nugetUrl)) ?? (await GetPackageVersionListFromUrlAsync(commercialUrl));
        }

        private async Task<List<string>> GetPackageVersionListFromUrlAsync(string url)
        {
            try
            {
                var client = _cliHttpClientFactory.CreateClient(needsAuthentication: false);

                using (var responseMessage = await client.GetHttpResponseMessageWithRetryAsync(
                    url,
                    cancellationToken: CancellationTokenProvider.Token,
                    logger: Logger
                ))
                {
                    await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(responseMessage);
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<NuGetVersionResultDto>(responseContent).Versions;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<string> GetNuGetUrlForCommercialPackage(string packageId)
        {
            if (_apiKeyResult == null)
            {
                _apiKeyResult = await _apiKeyService.GetApiKeyOrNullAsync();

                if (_apiKeyResult == null)
                {
                    return null;
                }
            }

            return CliUrls.GetNuGetPackageInfoUrl(_apiKeyResult.ApiKey, packageId);
        }

        public class NuGetVersionResultDto
        {
            [JsonProperty("versions")]
            public List<string> Versions { get; set; }
        }
    }
}
