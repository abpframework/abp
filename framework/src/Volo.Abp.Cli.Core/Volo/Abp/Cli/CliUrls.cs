namespace Volo.Abp.Cli
{
    public static class CliUrls
    {
#if DEBUG
        public const string WwwAbpIo = "https://localhost:44328/";

        public const string AccountAbpIo = "https://localhost:44333/";

        public const string NuGetRootPath = "https://localhost:44373/";
#else
        public const string WwwAbpIo = "https://abp.io/";
        
        public const string AccountAbpIo = "https://account.abp.io/";
       
        public const string NuGetRootPath = "https://nuget.abp.io/";
#endif

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
