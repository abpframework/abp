using System;
using System.IO;

namespace Volo.Abp.Cli
{
    public static class CliPaths
    {
        public static string TemplateCachePath => Path.Combine(AbpRootPath, "templates");
        public static string CliLogPath => Path.Combine(AbpRootPath, "cli", "logs");

        private static readonly string AbpRootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".abp");
    }
}
