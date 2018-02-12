using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public abstract class PermissionValueProvider : IPermissionValueProvider, ISingletonDependency
    {
        public abstract string Name { get; }

        public ILoggerFactory LoggerFactory { get; set; }

        protected IPermissionStore PermissionStore { get; }

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        protected PermissionValueProvider(IPermissionStore permissionStore)
        {
            PermissionStore = permissionStore;
        }

        public abstract Task<bool?> IsGrantedAsync(PermissionDefinition permission, string providerName, string providerKey);

        public abstract Task SetAsync(PermissionDefinition permission, bool isGranted, string providerKey);

        public abstract Task ClearAsync(PermissionDefinition permission, string providerKey);
    }
}