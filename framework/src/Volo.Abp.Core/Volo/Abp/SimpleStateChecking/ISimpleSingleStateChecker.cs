using System.Threading.Tasks;

namespace Volo.Abp.SimpleStateChecking
{
    public interface ISimpleSingleStateChecker<TState> : ISimpleStateChecker<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        Task<bool> IsEnabledAsync(SimpleSingleStateCheckerContext<TState> context);
    }
}
