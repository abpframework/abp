using System;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Cli;

public static class CliUrls
{
    public static string WwwAbpIo
    {
        get => WwwAbpIoProduction;
        set => WwwAbpIoProduction = value;
    }

    public static string AccountAbpIo
    {
        get => AccountAbpIoProduction;
        set => AccountAbpIoProduction = value;
    }

    public static string NuGetRootPath
    {
        get => NuGetRootPathProduction;
        set => NuGetRootPathProduction = value;
    }

    public static string LatestVersionCheckFullPath = "https://raw.githubusercontent.com/abpframework/abp/dev/latest-versions.json";

    public static string WwwAbpIoProduction = "https://abp.io/";
    public static string AccountAbpIoProduction = "https://account.abp.io/";
    public static string NuGetRootPathProduction = "https://nuget.abp.io/";

    public static string WwwAbpIoDevelopment = "https://localhost:44328/";
    public static string AccountAbpIoDevelopment = "https://localhost:44333/";
    public static string NuGetRootPathDevelopment = "https://localhost:44373/";

    public static string GetNuGetServiceIndexUrl(string apiKey)
    {
        return $"{NuGetRootPath}{apiKey}/v3/index.json";
    }

    public static string GetNuGetPackageInfoUrl(string apiKey, string packageId)
    {
        return $"{NuGetRootPath}{apiKey}/v3/package/{packageId}/index.json";
    }

    public static string GetNuGetPackageSearchUrl(string apiKey, string packageId)
    {
        return $"{NuGetRootPath}{apiKey}/v3/search?q={packageId}";
    }

    public static string GetApiDefinitionUrl(string url, ApplicationApiDescriptionModelRequestDto model = null)
    {
        url = url.EnsureEndsWith('/');
        return $"{url}api/abp/api-definition{(model != null ? model.IncludeTypes ? "?includeTypes=true" : string.Empty : string.Empty)}";
    }
}
