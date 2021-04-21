using System;
using System.Collections.Generic;
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
            return await InternalIsEnabledAsync(state, true);
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

                foreach (var stateChecker in multipleStateCheckers)
                {
                    var context = new SimpleMultipleStateCheckerContext<TState>(
                        scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>(),
                        states.Where(x => x.SimpleStateCheckers.Contains(stateChecker)).ToArray());

                    foreach (var x in await stateChecker.IsEnabledAsync(context))
                    {
                        result[x.Key] = x.Value;
                    }

                    if (result.Values.All(x => !x))
                    {
                        return result;
                    }
                }

                foreach (ISimpleMultipleStateChecker<TState> globalStateChecker in Options.GlobalSimpleStateCheckers
                    .Where(x => typeof(ISimpleMultipleStateChecker<TState>).IsAssignableFrom(x))
                    .Select(x => ServiceProvider.GetRequiredService(x)))
                {
                    var context = new SimpleMultipleStateCheckerContext<TState>(
                        scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>(),
                        states.Where(x => result.Any(y => y.Key.Equals(x) && y.Value)).ToArray());

                    foreach (var x in await globalStateChecker.IsEnabledAsync(context))
                    {
                        result[x.Key] = x.Value;
                    }
                }

                foreach (var state in states)
                {
                    if (result[state])
                    {
                        result[state] = await InternalIsEnabledAsync(state, false);
                    }
                }

                return result;
            }
        }

        protected virtual async Task<bool> InternalIsEnabledAsync(TState state, bool useMultipleStateChecker)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new SimpleStateCheckerContext<TState>(scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>(), state);

                foreach (var provider in state.SimpleStateCheckers.WhereIf(!useMultipleStateChecker, x => x is not ISimpleMultipleStateChecker<TState>))
                {
                    if (!await provider.IsEnabledAsync(context))
                    {
                        return false;
                    }
                }

                foreach (ISimpleStateChecker<TState> provider in Options.GlobalSimpleStateCheckers
                    .WhereIf(!useMultipleStateChecker, x => !typeof(ISimpleMultipleStateChecker<TState>).IsAssignableFrom(x))
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
    }
}
