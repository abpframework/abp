using System.Threading.Tasks;

namespace Volo.Abp.SimpleStateChecking
{
    public interface ISimpleMultipleStateChecker<TState>: ISimpleStateChecker<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(SimpleMultipleStateCheckerContext<TState> context);
    }
}
