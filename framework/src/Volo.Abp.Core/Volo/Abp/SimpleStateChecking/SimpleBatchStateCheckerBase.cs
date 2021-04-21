using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.SimpleStateChecking
{
    public abstract class SimpleBatchStateCheckerBase<TState>: ISimpleBatchStateChecker<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        public async Task<bool> IsEnabledAsync(SimpleStateCheckerContext<TState> context)
        {
            return (await IsEnabledAsync(new SimpleBatchStateCheckerContext<TState>(context.ServiceProvider, new[] {context.State}))).Values.All(x => x);
        }

        public abstract Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(SimpleBatchStateCheckerContext<TState> context);
    }
}
