using System.Threading.Tasks;

namespace Volo.Abp.UI.Navigation;

/// <summary>
/// Provides a way to modify menu item URLs after the menu is fully configured.
/// Implementations can transform URLs based on the current request context
/// (e.g. adding a culture prefix for URL-based localization).
/// </summary>
public interface IMenuItemUrlProvider
{
    Task HandleAsync(MenuItemUrlProviderContext context);
}
