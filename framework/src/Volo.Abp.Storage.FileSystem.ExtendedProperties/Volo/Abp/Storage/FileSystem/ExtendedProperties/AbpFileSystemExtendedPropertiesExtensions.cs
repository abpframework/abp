using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Storage.FileSystem.ExtendedProperties.Internal;

namespace Volo.Abp.Storage.FileSystem.ExtendedProperties
{
    public static class AbpFileSystemExtendedPropertiesExtensions
    {
        public static IServiceCollection AddFileSystemExtendedProperties([NotNull] this IServiceCollection services,
            Action<AbpFileSystemExtendedPropertiesOptions> configure = null)
        {
            if (configure == null) configure = o => { };

            services.Configure(configure);
            services.AddTransient<IExtendedPropertiesProvider, ExtendedPropertiesProvider>();
            return services;
        }
    }
}