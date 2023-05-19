using System;

namespace Volo.Abp.SimpleStateChecking;

public class SimpleBatchStateCheckerContext<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public IServiceProvider ServiceProvider { get; }

    public TState[] States { get; }

    public SimpleBatchStateCheckerContext(IServiceProvider serviceProvider, TState[] states)
    {
        ServiceProvider = serviceProvider;
        States = states;
    }
}
