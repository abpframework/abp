using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

public interface IToolbarContributor
{
    Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
}
