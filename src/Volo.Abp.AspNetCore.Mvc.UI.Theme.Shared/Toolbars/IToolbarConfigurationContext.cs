using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public interface IToolbarConfigurationContext : IServiceProviderAccessor
    {
        Toolbar Toolbar { get; }
    }
}