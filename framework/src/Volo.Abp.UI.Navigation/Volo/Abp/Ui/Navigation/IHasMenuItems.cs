using Volo.Abp.UI.Navigation;

namespace Volo.Abp.UI.Navigation;

public interface IHasMenuItems
{
    /// <summary>
    /// Menu items.
    /// </summary>
    ApplicationMenuItemList Items { get; }
}
