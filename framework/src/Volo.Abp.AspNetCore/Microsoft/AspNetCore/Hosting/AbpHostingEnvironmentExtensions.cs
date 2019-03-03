using System;
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Hosting
{
    public static class AbpHostingEnvironmentExtensions
    {
        public static IConfigurationRoot BuildConfiguration(
            this IHostingEnvironment env,
            ConfigurationBuilderOptions options = null)
        {
            options = options ?? new ConfigurationBuilderOptions();

            if (options.BasePath.IsNullOrEmpty())
            {
                options.BasePath = env.ContentRootPath;
            }

            if (options.EnvironmentName.IsNullOrEmpty())
            {
                options.EnvironmentName = env.EnvironmentName;
            }

            return ConfigurationHelper.BuildConfiguration(options);
        }
    }
}
