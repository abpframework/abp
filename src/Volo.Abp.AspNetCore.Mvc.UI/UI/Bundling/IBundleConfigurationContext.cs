using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleConfigurationContext : IServiceProviderAccessor
    {
        List<string> Files { get; }

        ITheme Theme { get; }
    }
}