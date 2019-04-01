using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Client
{
    public static class RemoteServiceConfigurationExtensions
    {
        public const string IdentityClientName = "IdentityClient";
        public const string UseCurrentAccessTokenName = "UseCurrentAccessToken";

        [CanBeNull]
        public static string GetIdentityClient([NotNull] this RemoteServiceConfiguration configuration)
        {
            Check.NotNullOrEmpty(configuration, nameof(configuration));

            return configuration.GetOrDefault(IdentityClientName);
        }

        public static RemoteServiceConfiguration SetIdentityClient([NotNull] this RemoteServiceConfiguration configuration, [CanBeNull] string value)
        {
            configuration[IdentityClientName] = value;
            return configuration;
        }

        [CanBeNull]
        public static bool? GetUseCurrentAccessToken([NotNull] this RemoteServiceConfiguration configuration)
        {
            Check.NotNullOrEmpty(configuration, nameof(configuration));

            var value = configuration.GetOrDefault(UseCurrentAccessTokenName);
            if (value == null)
            {
                return null;
            }

            return bool.Parse(value);
        }

        public static RemoteServiceConfiguration SetUseCurrentAccessToken([NotNull] this RemoteServiceConfiguration configuration, [CanBeNull] bool? value)
        {
            if (value == null)
            {
                configuration.Remove(UseCurrentAccessTokenName);
            }
            else
            {
                configuration[UseCurrentAccessTokenName] = value.Value.ToString().ToLowerInvariant();
            }

            return configuration;
        }
    }
}
