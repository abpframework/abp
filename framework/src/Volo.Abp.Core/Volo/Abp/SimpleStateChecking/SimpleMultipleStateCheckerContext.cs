using System;

namespace Volo.Abp.SimpleStateChecking
{
    public class SimpleMultipleStateCheckerContext<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        public IServiceProvider ServiceProvider { get; }

        public TState[] States { get; }

        public SimpleMultipleStateCheckerContext(IServiceProvider serviceProvider, TState[] states)
        {
            ServiceProvider = serviceProvider;
            States = states;
        }
    }
}
