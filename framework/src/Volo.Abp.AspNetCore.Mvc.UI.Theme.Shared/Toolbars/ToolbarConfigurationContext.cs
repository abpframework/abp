using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

public class ToolbarConfigurationContext : IToolbarConfigurationContext
{
    public IServiceProvider ServiceProvider { get; }

    private readonly IAbpLazyServiceProvider _lazyServiceProvider;

    public IAuthorizationService AuthorizationService => _lazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

    public IStringLocalizerFactory StringLocalizerFactory => _lazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

    public ITheme Theme { get; }

    public Toolbar Toolbar { get; }

    public ToolbarConfigurationContext(ITheme currentTheme, Toolbar toolbar, IServiceProvider serviceProvider)
    {
        Theme = currentTheme;
        Toolbar = toolbar;
        ServiceProvider = serviceProvider;
        _lazyServiceProvider = ServiceProvider.GetRequiredService<IAbpLazyServiceProvider>();
    }

    public Task<bool> IsGrantedAsync(string policyName)
    {
        return AuthorizationService.IsGrantedAsync(policyName);
    }

    [CanBeNull]
    public IStringLocalizer GetDefaultLocalizer()
    {
        return StringLocalizerFactory.CreateDefaultOrNull();
    }

    [NotNull]
    public IStringLocalizer GetLocalizer<T>()
    {
        return StringLocalizerFactory.Create<T>();
    }

    [NotNull]
    public IStringLocalizer GetLocalizer(Type resourceType)
    {
        return StringLocalizerFactory.Create(resourceType);
    }
}
