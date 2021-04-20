using System.Threading.Tasks;

namespace Volo.Abp.SimpleStateChecking
{
    public interface ISimpleStateCheckerManager<in TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        Task<bool> IsEnabledAsync(TState state);
    }
}
