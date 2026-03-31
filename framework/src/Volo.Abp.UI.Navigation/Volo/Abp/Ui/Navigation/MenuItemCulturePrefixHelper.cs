namespace Volo.Abp.UI.Navigation;

public static class MenuItemCulturePrefixHelper
{
    public static void PrependCulturePrefix(IHasMenuItems menuWithItems, string prefix)
    {
        foreach (var item in menuWithItems.Items)
        {
            if (item.Url != null)
            {
                if (item.Url.StartsWith("~/"))
                {
                    item.Url = "~" + prefix + item.Url.Substring(1);
                }
                else if (item.Url.StartsWith("/"))
                {
                    item.Url = prefix + item.Url;
                }
            }

            PrependCulturePrefix(item, prefix);
        }
    }
}
