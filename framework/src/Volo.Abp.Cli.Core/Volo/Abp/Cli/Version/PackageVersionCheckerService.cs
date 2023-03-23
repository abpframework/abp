using System;
using Newtonsoft.Json;
using NuGet.Versioning;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Volo.Abp.Cli.Version;

public class PackageVersionCheckerService : ITransientDependency
{
    public ILogger<VoloNugetPackagesVersionUpdater> Logger { get; set; }
    protected IJsonSerializer JsonSerializer { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }
    private readonly IApiKeyService _apiKeyService;
    private readonly CliHttpClientFactory _cliHttpClientFactory;
    private DeveloperApiKeyResult _apiKeyResult;

    public PackageVersionCheckerService(
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

    public async Task<bool> PackageExistAsync(string packageId, string version = null)
    {
        var versionList = await GetPackageVersionListAsync(packageId, false);

        if (versionList == null)
        {
            return false;
        }

        if (version == null)
        {
            return versionList.Any();
        }

        return versionList.Contains(version);
    }

    public async Task<LatestVersionInfo> GetLatestVersionOrNullAsync(string packageId, bool includeNightly = false, bool includeReleaseCandidates = false)
    {
        if (!includeNightly && !includeReleaseCandidates && !packageId.Contains("LeptonX"))
        {
            var latestStableVersionResult = await GetLatestStableVersionOrNullAsync();
            if (latestStableVersionResult == null)
            {
                return null;
            }

            return SemanticVersion.TryParse(latestStableVersionResult.Version, out var semanticVersion) 
                ? new LatestVersionInfo(semanticVersion, latestStableVersionResult.Message) 
                : null;
        }
        
        var versionList = await GetPackageVersionListAsync(packageId, includeNightly);
        if (versionList == null)
        {
            return null;
        }

        List<SemanticVersion> versions;
        
        if (!includeNightly && includeReleaseCandidates)
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

        return versions.Any() 
            ? new LatestVersionInfo(versions.Max()) 
            : null;

    }

    public async Task<List<string>> GetPackageVersionListAsync(string packageId, bool includeNightly = false)
    {
        if (includeNightly)
        {
            return await GetPackageVersionsFromMyGet(packageId);
        }

        if (CommercialPackages.IsCommercial(packageId))
        {
            return await GetPackageVersionsFromAbpCommercialNuGetAsync(packageId);
        }
        else
        {
            var packagesFromNugetOrg = await GetPackageVersionsFromNuGetOrgAsync(packageId);
            if (packagesFromNugetOrg != null)
            {
                return packagesFromNugetOrg;
            }
        }

        return await GetPackageVersionsFromAbpCommercialNuGetAsync(packageId);
    }

    private async Task<List<string>> GetPackageVersionsFromAbpCommercialNuGetAsync(string packageId)
    {
        var url = await GetNuGetUrlForCommercialPackage(packageId);
        return await GetPackageVersionListFromUrlAsync(url);
    }

    private async Task<List<string>> GetPackageVersionsFromNuGetOrgAsync(string packageId)
    {
        var url = $"https://api.nuget.org/v3-flatcontainer/{packageId.ToLowerInvariant()}/index.json";
        return await GetPackageVersionListFromUrlAsync(url);
    }

    private async Task<List<string>> GetPackageVersionsFromMyGet(string packageId)
    {
        var url = $"https://www.myget.org/F/abp-nightly/api/v3/flatcontainer/{packageId.ToLowerInvariant()}/index.json";
        return await GetPackageVersionListFromUrlAsync(url);
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
                if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    //the package doesn't exist...
                    return null;
                }
                
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

    private async Task<LatestStableVersionResult> GetLatestStableVersionOrNullAsync()
    {
        try
        {
            var client = _cliHttpClientFactory.CreateClient(clientName: CliConsts.GithubHttpClientName, needsAuthentication: false);

            using (var responseMessage = await client.GetHttpResponseMessageWithRetryAsync(
                       CliUrls.LatestVersionCheckFullPath,
                       cancellationToken: CancellationTokenProvider.Token,
                       logger: Logger
                   ))
            {
                await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(responseMessage);

                var content = await responseMessage.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<LatestStableVersionResult>>(content);
                
                return result.FirstOrDefault(x => x.Type.ToLowerInvariant() == "stable");
            }
        }
        catch
        {
            return null;
        }
    }

    public class NuGetVersionResultDto
    {
        [JsonProperty("versions")]
        public List<string> Versions { get; set; }
    }

    public class LatestStableVersionResult
    {
        public string Version { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string Type { get; set; }
        
        public string Message { get; set; }
    }
}
