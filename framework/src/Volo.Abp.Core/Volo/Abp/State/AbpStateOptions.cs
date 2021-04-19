using Volo.Abp.Collections;

namespace Volo.Abp.State
{
    public class AbpStateOptions<TState>
        where TState : IHasState<TState>
    {
        public ITypeList<IStateProvider<TState>> GlobalStateProviders { get; }

        public AbpStateOptions()
        {
            GlobalStateProviders = new TypeList<IStateProvider<TState>>();
        }

    }
}
