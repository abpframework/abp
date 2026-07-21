namespace Volo.Abp.UI.Navigation;

public class MenuItemUrlProviderContext
{
    public ApplicationMenu Menu { get; }

    public MenuItemUrlProviderContext(ApplicationMenu menu)
    {
        Menu = menu;
    }
}
