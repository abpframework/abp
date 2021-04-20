using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SimpleStateChecking
{
    public class SimpleStateCheckerManager<TState> : ISimpleStateCheckerManager<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        protected IServiceProvider ServiceProvider { get; }

        protected AbpSimpleStateCheckerOptions<TState> Options { get; }

        public SimpleStateCheckerManager(IServiceProvider serviceProvider, IOptions<AbpSimpleStateCheckerOptions<TState>> options)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
        }

        public virtual async Task<bool> IsEnabledAsync(TState state)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new SimpleStateCheckerContext<TState>(scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>(), state);

                foreach (var provider in state.SimpleStateCheckers)
                {
                    if (!await provider.IsEnabledAsync(context))
                    {
                        return false;
                    }
                }

                foreach (ISimpleStateChecker<TState> provider in Options.GlobalSimpleStateCheckers.Select(x => ServiceProvider.GetRequiredService(x)))
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
