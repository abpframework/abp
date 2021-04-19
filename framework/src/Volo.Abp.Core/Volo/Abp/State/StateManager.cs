using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.State
{
    public class StateManager<TState> : IStateManager<TState>
        where TState : IHasState<TState>
    {
        protected IServiceProvider ServiceProvider { get; }

        protected AbpStateOptions<TState> Options { get; }

        public StateManager(IServiceProvider serviceProvider, IOptions<AbpStateOptions<TState>> options)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
        }

        public virtual async Task<bool> IsEnabledAsync(TState state)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new StateCheckContext<TState>(scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>(), state);

                foreach (var provider in state.StateProviders)
                {
                    if (!await provider.IsEnabledAsync(context))
                    {
                        return false;
                    }
                }

                foreach (IStateProvider<TState> provider in Options.GlobalStateProviders.Select(x => ServiceProvider.GetRequiredService(x)))
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
