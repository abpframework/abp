using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Toolbars
{
    public interface IToolbarContributor
    {
        Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
    }
}