using System;

namespace Volo.Abp.State
{
    public class StateCheckContext<TState>
        where TState : IHasState<TState>
    {
        public IServiceProvider ServiceProvider { get; }

        public TState State { get; }

        public StateCheckContext(IServiceProvider serviceProvider, TState state)
        {
            ServiceProvider = serviceProvider;
            State = state;
        }
    }
}
