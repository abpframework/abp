using System;

namespace Volo.Abp.SimpleStateChecking
{
    public class SimpleSingleStateCheckerContext<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        public IServiceProvider ServiceProvider { get; }

        public TState State { get; }

        public SimpleSingleStateCheckerContext(IServiceProvider serviceProvider, TState state)
        {
            ServiceProvider = serviceProvider;
            State = state;
        }
    }
}
