using System.Collections.Generic;

namespace Volo.Abp.SimpleStateChecking
{
    public interface IHasSimpleStateCheckers<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        List<ISimpleStateChecker<TState>> SimpleStateCheckers { get; }
    }
}
