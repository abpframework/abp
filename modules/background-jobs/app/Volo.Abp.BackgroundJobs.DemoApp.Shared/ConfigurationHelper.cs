using System.IO;
using Microsoft.Extensions.Configuration;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot BuildConfiguration()
        {
            const string fileName = "appsettings.json";

            var directory = Directory.GetCurrentDirectory();

            while (!File.Exists(Path.Combine(directory, fileName)))
            {
                var parentDirectory = new DirectoryInfo(directory).Parent;
                if (parentDirectory == null)
                {
                    break;
                }

                directory = parentDirectory.FullName;
            }

            if (File.Exists(Path.Combine(directory, fileName)))
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(directory)
                    .AddJsonFile(fileName, optional: false);

                return builder.Build();
            }

            return null;
        }
    }
}
