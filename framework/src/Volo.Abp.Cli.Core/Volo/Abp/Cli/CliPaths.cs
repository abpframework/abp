using System;
using System.IO;

namespace Volo.Abp.Cli
{
    public static class CliPaths
    {
        public static string TemplateCache => Path.Combine(AbpRootPath, "templates"); //TODO: Move somewhere else?
        public static string Log => Path.Combine(AbpRootPath, "cli", "logs");
        public static string Root => Path.Combine(AbpRootPath, "cli");
        public static string AccessToken => Path.Combine(AbpRootPath, "cli", "access-token.bin");

        private static readonly string AbpRootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".abp");
    }
}
