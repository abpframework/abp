using Newtonsoft.Json;
using NuGet.Versioning;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Auth;
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
        private List<string> _proPackageList;
        private DeveloperApiKeyResult _apiKeyResult;

        public NuGetService(
            IJsonSerializer jsonSerializer,
            IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
            ICancellationTokenProvider cancellationTokenProvider,
            IApiKeyService apiKeyService)
        {
            JsonSerializer = jsonSerializer;
            RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
            CancellationTokenProvider = cancellationTokenProvider;
            _apiKeyService = apiKeyService;
            Logger = NullLogger<VoloNugetPackagesVersionUpdater>.Instance;
        }

        public async Task<SemanticVersion> GetLatestVersionOrNullAsync(string packageId, bool includeNightly = false, bool includeReleaseCandidates = false)
        {
            var versionList = await GetPackageVersionListAsync(packageId, includeNightly, includeReleaseCandidates);

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
            if (AuthService.IsLoggedIn())
            {
                _proPackageList ??= await GetProPackageListAsync();
            }

            string url;
            if (includeNightly)
            {
                url = $"https://www.myget.org/F/abp-nightly/api/v3/flatcontainer/{packageId.ToLowerInvariant()}/index.json";
            }
            else if (_proPackageList?.Contains(packageId) ?? false)
            {
                url = await GetNuGetUrlForCommercialPackage(packageId);
            }
            else
            {
                url = $"https://api.nuget.org/v3-flatcontainer/{packageId.ToLowerInvariant()}/index.json";
            }

            using (var client = new CliHttpClient(setBearerToken: false))
            {
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
        }

        private async Task<string> GetNuGetUrlForCommercialPackage(string packageId)
        {
            if (_apiKeyResult == null)
            {
                _apiKeyResult = await _apiKeyService.GetApiKeyOrNullAsync();
            }

            return CliUrls.GetNuGetPackageInfoUrl(_apiKeyResult.ApiKey, packageId);
        }

        private async Task<List<string>> GetProPackageListAsync()
        {
            using var client = new CliHttpClient();

            var url = $"{CliUrls.WwwAbpIo}api/app/nugetPackage/proPackageNames";

            using (var responseMessage = await client.GetHttpResponseMessageWithRetryAsync(
                url: url,
                cancellationToken: CancellationTokenProvider.Token,
                logger: Logger
            ))
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<string>>(await responseMessage.Content.ReadAsStringAsync());
                }

                var exceptionMessage = "Remote server returns '" + (int)responseMessage.StatusCode + "-" + responseMessage.ReasonPhrase + "'. ";
                var remoteServiceErrorMessage = await RemoteServiceExceptionHandler.GetAbpRemoteServiceErrorAsync(responseMessage);

                if (remoteServiceErrorMessage != null)
                {
                    exceptionMessage += remoteServiceErrorMessage;
                }

                Logger.LogError(exceptionMessage);
                return null;
            }
        }

        public class NuGetVersionResultDto
        {
            [JsonProperty("versions")]
            public List<string> Versions { get; set; }
        }
    }
}