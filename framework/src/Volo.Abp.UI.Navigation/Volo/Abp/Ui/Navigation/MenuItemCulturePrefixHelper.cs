using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.UI.Navigation;

public class MenuItemCulturePrefixHelper : IMenuItemCulturePrefixHelper, ITransientDependency
{
    public virtual Task PrependCulturePrefixAsync(IHasMenuItems menuWithItems, string prefix)
    {
        PrependCulturePrefix(menuWithItems, prefix);
        return Task.CompletedTask;
    }

    protected virtual void PrependCulturePrefix(IHasMenuItems menuWithItems, string prefix)
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
