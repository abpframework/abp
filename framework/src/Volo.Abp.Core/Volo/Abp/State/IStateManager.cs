using System.Threading.Tasks;

namespace Volo.Abp.State
{
    public interface IStateManager<in TState>
        where TState : IHasState<TState>
    {
        Task<bool> IsEnabledAsync(TState state);
    }
}
