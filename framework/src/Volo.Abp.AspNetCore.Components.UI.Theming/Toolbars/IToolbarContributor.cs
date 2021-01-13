using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.UI.Theming.Toolbars
{
    public interface IToolbarContributor
    {
        Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
    }
}