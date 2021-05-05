using Volo.Abp.Collections;

namespace Volo.Abp.SimpleStateChecking
{
    public class AbpSimpleStateCheckerOptions<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        public ITypeList<ISimpleStateChecker<TState>> GlobalStateCheckers { get; }

        public AbpSimpleStateCheckerOptions()
        {
            GlobalStateCheckers = new TypeList<ISimpleStateChecker<TState>>();
        }
    }
}
