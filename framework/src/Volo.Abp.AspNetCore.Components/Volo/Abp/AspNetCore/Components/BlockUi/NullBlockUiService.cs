using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.BlockUi;

public class NullBlockUiService : IBlockUiService, ISingletonDependency
{
    public Task Block(string? selectors, bool busy = false)
    {
        return Task.CompletedTask;
    }

    public Task UnBlock()
    {
        return Task.CompletedTask;
    }
}
