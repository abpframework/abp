using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class AbpAspNetCoreHostBuilderExtensions
    {
        public const string AppSettingsSecretJsonPath = "appsettings.secrets.json";

        public static IHostBuilder AddAppSettingsSecretsJson(
            this IHostBuilder hostBuilder,
            bool optional = true,
            bool reloadOnChange = true,
            string path = AppSettingsSecretJsonPath)
        {
            return hostBuilder.ConfigureAppConfiguration(appConfig =>
            {
                appConfig.AddJsonFile(
                    path: AppSettingsSecretJsonPath,
                    optional: optional,
                    reloadOnChange: reloadOnChange
                );
            });
        }
    }
}
