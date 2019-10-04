namespace Volo.Abp.Cli
{
    public static class CliUrls
    {
#if DEBUG
        public const string WwwAbpIo = "https://localhost:44328/";
        public const string AccountAbpIo = "https://localhost:44333/";
#else
        public const string WwwAbpIo = "https://abp.io/";
        public const string AccountAbpIo = "https://account.abp.io/";
#endif
    }
}
