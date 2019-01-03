using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Client
{
    public static class RemoteServiceConfigurationExtensions
    {
        [CanBeNull]
        public static string GetIdentityClient([NotNull] this RemoteServiceConfiguration configuration)
        {
            Check.NotNullOrEmpty(configuration, nameof(configuration));

            return configuration.GetOrDefault("IdentityClient");
        }

        public static RemoteServiceConfiguration SetIdentityClient([NotNull] this RemoteServiceConfiguration configuration, [CanBeNull] string value)
        {
            configuration["IdentityClient"] = value;
            return configuration;
        }
    }
}
