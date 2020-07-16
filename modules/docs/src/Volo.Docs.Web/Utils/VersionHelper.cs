namespace Volo.Docs.Utils
{
    public static class VersionHelper
    {

        public static bool IsPreRelease(string version)
        {
            return (version?.Split("-").Length ?? 0) > 1;
        }
    }
}
