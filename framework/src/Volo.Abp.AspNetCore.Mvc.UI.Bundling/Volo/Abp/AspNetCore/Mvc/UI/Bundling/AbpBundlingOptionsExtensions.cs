using Microsoft.Extensions.Hosting;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public static class AbpBundlingOptionsExtensions
    {
        public static bool IsBundlingEnabled(this AbpBundlingOptions options, IHostEnvironment environment)
        {
            switch (options.Mode)
            {
                case BundlingMode.None:
                    return false;
                case BundlingMode.Bundle:
                case BundlingMode.BundleAndMinify:
                    return true;
                case BundlingMode.Auto:
                    return !environment.IsDevelopment();
                default:
                    throw new AbpException($"Unhandled {nameof(BundlingMode)}: {options.Mode}");
            }
        }

        public static bool IsMinficationEnabled(this AbpBundlingOptions options, IHostEnvironment environment)
        {
            switch (options.Mode)
            {
                case BundlingMode.None:
                case BundlingMode.Bundle:
                    return false;
                case BundlingMode.BundleAndMinify:
                    return true;
                case BundlingMode.Auto:
                    return !environment.IsDevelopment();
                default:
                    throw new AbpException($"Unhandled {nameof(BundlingMode)}: {options.Mode}");
            }
        }
    }
}
