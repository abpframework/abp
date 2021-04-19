using System.Collections.Generic;

namespace Volo.Abp.State
{
    public interface IHasState<TState>
        where TState : IHasState<TState>
    {
        List<IStateProvider<TState>> StateProviders { get; }
    }
}
