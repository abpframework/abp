﻿using System;

namespace Volo.Abp.Cli;

public static class CliUrls
{
    public const string WwwAbpIo = WwwAbpIoProduction;
    public const string AccountAbpIo = AccountAbpIoProduction;
    public const string NuGetRootPath = NuGetRootPathProduction;

    public const string WwwAbpIoProduction = "https://abp.io/";
    public const string AccountAbpIoProduction = "https://account.abp.io/";
    public const string NuGetRootPathProduction = "https://nuget.abp.io/";

    public const string WwwAbpIoDevelopment = "https://localhost:44328/";
    public const string AccountAbpIoDevelopment = "https://localhost:44333/";
    public const string NuGetRootPathDevelopment = "https://localhost:44373/";

    public static string GetNuGetServiceIndexUrl(string apiKey)
    {
        return $"{NuGetRootPath}{apiKey}/v3/index.json";
    }

    public static string GetNuGetPackageInfoUrl(string apiKey, string packageId)
    {
        return $"{NuGetRootPath}{apiKey}/v3/package/{packageId}/index.json";
    }

    public static string GetApiDefinitionUrl(string url)
    {
        url = url.EnsureEndsWith('/');
        return $"{url}api/abp/api-definition";
    }
}
