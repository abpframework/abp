using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.BlockUi;

public interface IBlockUiService
{
    Task Block(string? selectors, bool busy = false);

    Task UnBlock();
}
