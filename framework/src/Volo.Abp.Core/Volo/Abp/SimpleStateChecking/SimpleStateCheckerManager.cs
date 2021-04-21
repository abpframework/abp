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
            if (!state.SimpleStateCheckers.Any(x => x is ISimpleSingleStateChecker<TState>) &&
                !Options.GlobalSimpleStateCheckers.Any(x => typeof(ISimpleSingleStateChecker<TState>).IsAssignableFrom(x)))
            {
                return true;
            }

            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new SimpleSingleStateCheckerContext<TState>(scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>(), state);

                foreach (var provider in state.SimpleStateCheckers
                    .Where(x => x is ISimpleSingleStateChecker<TState>)
                    .Cast<ISimpleSingleStateChecker<TState>>())
                {
                    if (!await provider.IsEnabledAsync(context))
                    {
                        return false;
                    }
                }

                foreach (ISimpleSingleStateChecker<TState> provider in Options.GlobalSimpleStateCheckers
                    .Where(x => typeof(ISimpleSingleStateChecker<TState>).IsAssignableFrom(x))
                    .Select(x => ServiceProvider.GetRequiredService(x)))
                {
                    if (!await provider.IsEnabledAsync(context))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public virtual async Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(TState[] states)
        {
            var result = new SimpleStateCheckerResult<TState>(states);

            using (var scope = ServiceProvider.CreateScope())
            {
                var multipleStateCheckers = states.SelectMany(x => x.SimpleStateCheckers)
                    .Where(x => x is ISimpleMultipleStateChecker<TState>)
                    .Cast<ISimpleMultipleStateChecker<TState>>()
                    .GroupBy(x => x)
                    .Select(x => x.Key);

                foreach (var state in multipleStateCheckers)
                {
                    var context = new SimpleMultipleStateCheckerContext<TState>(
                        scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>(),
                        states.Where(x => x.SimpleStateCheckers.Contains(state)).ToArray());

                    foreach (var x in await state.IsEnabledAsync(context))
                    {
                        result[x.Key] = x.Value;
                    }

                    if (result.Values.All(x => !x))
                    {
                        return result;
                    }
                }

                foreach (var state in states)
                {
                    if (result[state])
                    {
                        result[state] = await IsEnabledAsync(state);
                    }
                }

                return result;
            }
        }
    }
}
