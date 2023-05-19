using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theming;

public class DefaultThemeManager : IThemeManager, IScopedDependency, IServiceProviderAccessor
{
    private const string CurrentThemeHttpContextKey = "__AbpCurrentTheme";

    public IServiceProvider ServiceProvider { get; }
    public ITheme CurrentTheme => GetCurrentTheme();

    protected IThemeSelector ThemeSelector { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }

    public DefaultThemeManager(
        IServiceProvider serviceProvider,
        IThemeSelector themeSelector,
        IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
        ServiceProvider = serviceProvider;
        ThemeSelector = themeSelector;
    }

    protected virtual ITheme GetCurrentTheme()
    {
        var preSelectedTheme = HttpContextAccessor.HttpContext.Items[CurrentThemeHttpContextKey] as ITheme;

        if (preSelectedTheme == null)
        {
            preSelectedTheme = (ITheme)ServiceProvider.GetRequiredService(ThemeSelector.GetCurrentThemeInfo().ThemeType);
            HttpContextAccessor.HttpContext.Items[CurrentThemeHttpContextKey] = preSelectedTheme;
        }

        return preSelectedTheme;
    }
}
