using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public interface IToolbarConfigurationContext : IServiceProviderAccessor
    {
        ITheme Theme { get; }

        Toolbar Toolbar { get; }

        IAuthorizationService AuthorizationService { get; }

        IStringLocalizerFactory StringLocalizerFactory { get; }
    }
}
