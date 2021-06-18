using System;
using System.IO;
using System.Text;

namespace Volo.Abp.Cli
{
    public static class CliPaths
    {
        public static string TemplateCache => Path.Combine(AbpRootPath, "templates"); //TODO: Move somewhere else?
        public static string Log => Path.Combine(AbpRootPath, "cli", "logs");
        public static string Root => Path.Combine(AbpRootPath, "cli");
        public static string AccessToken => Path.Combine(AbpRootPath, "cli", "access-token.bin");
        public static string Build => Path.Combine(AbpRootPath, "build");
        public static string Lic => Path.Combine(Path.GetTempPath(), Encoding.ASCII.GetString(new byte[] { 65, 98, 112, 76, 105, 99, 101, 110, 115, 101, 46, 98, 105, 110 }));

        private static readonly string AbpRootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".abp");
    }
}
