using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionStateManager : IPermissionStateManager, ITransientDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        protected AbpPermissionOptions Options { get; }

        public PermissionStateManager(IServiceProvider serviceProvider, IOptions<AbpPermissionOptions> options)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
        }

        public async Task<bool> IsEnabledAsync(PermissionDefinition permission)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new PermissionStateContext
                {
                    Permission = permission,
                    ServiceProvider = scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>()
                };

                foreach (var provider in permission.StateProviders)
                {
                    if (!await provider.IsEnabledAsync(context))
                    {
                        return false;
                    }
                }

                foreach (IPermissionStateProvider provider in Options.GlobalStateProviders.Select(x => ServiceProvider.GetRequiredService(x)))
                {
                    if (!await provider.IsEnabledAsync(context))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
