using Volo.Abp.Collections;

namespace Volo.Abp.SimpleStateChecking
{
    public class AbpSimpleStateCheckerOptions<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        public ITypeList<ISimpleStateChecker<TState>> GlobalSimpleStateCheckers { get; }

        public AbpSimpleStateCheckerOptions()
        {
            GlobalSimpleStateCheckers = new TypeList<ISimpleStateChecker<TState>>();
        }
    }
}
