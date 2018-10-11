using System.Reflection;

namespace Microsoft.Extensions.Configuration
{
    public class AbpAspNetCoreConfigurationOptions
    {
        /// <summary>
        /// Used to set assembly which is used to get the user secret id for the application.
        /// Use this or <see cref="UserSecretsId"/> (higher priority)
        /// </summary>
        public Assembly UserSecretsAssembly { get; set; }

        /// <summary>
        /// Used to set user secret id for the application.
        /// Use this (higher priority) or <see cref="UserSecretsAssembly"/>
        /// </summary>
        public string UserSecretsId { get; set; }

        /// <summary>
        /// Default value: "appsettings".
        /// </summary>
        public string FileName { get; set; } = "appsettings";
    }
}
