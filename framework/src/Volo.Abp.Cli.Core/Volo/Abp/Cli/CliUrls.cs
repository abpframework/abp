namespace Volo.Abp.Cli
{
    public static class CliUrls
    {
#if DEBUG
        public const string WwwAbpIo = WwwAbpIoDevelopment;

        public const string AccountAbpIo = AccountAbpIoDevelopment;

        public const string NuGetRootPath = NuGetRootPathDevelopment;
#else
        public const string WwwAbpIoProduction = "https://abp.io/";
        public const string AccountAbpIoProduction = "https://account.abp.io/";
        public const string NuGetRootPathProduction = "https://nuget.abp.io/";
#endif

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
    }
}
