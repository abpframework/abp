using System.Threading.Tasks;

namespace Volo.Abp.UI.Navigation;

public interface IMenuItemCulturePrefixHelper
{
    Task PrependCulturePrefixAsync(IHasMenuItems menuWithItems, string prefix);
}
