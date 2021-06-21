using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars
{
    public interface IToolbarContributor
    {
        Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
    }
}