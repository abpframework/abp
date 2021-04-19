using System.Threading.Tasks;

namespace Volo.Abp.State
{
    public interface IStateProvider<TState>
        where TState : IHasState<TState>
    {
        Task<bool> IsEnabledAsync(StateCheckContext<TState> permission);
    }
}
