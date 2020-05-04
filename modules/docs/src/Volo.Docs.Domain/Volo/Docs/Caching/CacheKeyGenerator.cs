using Volo.Docs.Projects;

namespace Volo.Docs.Caching
{
    public static class CacheKeyGenerator
    {
        public static string GenerateProjectLanguageCacheKey(Project project)
        {
            return project.ShortName;
        }

        public static string GenerateProjectVersionsCacheKey(Project project)
        {
            return project.ShortName;
        }

        public static string GenerateDocumentResourceCacheKey(Project project, string resourceName, string languageCode, string version)
        {
            return $"Resource@{project.ShortName}#{languageCode}#{resourceName}#{version}";
        }

        public static string GenerateDocumentUpdateInfoCacheKey(Project project, string documentName, string languageCode, string version)
        {
            return $"DocumentUpdateInfo{project.Id}#{documentName}#{languageCode}#{version}";
        }
    }
}
