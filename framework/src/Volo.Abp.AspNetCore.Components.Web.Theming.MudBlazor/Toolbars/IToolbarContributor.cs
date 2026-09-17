using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Toolbars;

public interface IToolbarContributor
{
    Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
}
