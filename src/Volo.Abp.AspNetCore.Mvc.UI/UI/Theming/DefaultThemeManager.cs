using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theming
{
    public class DefaultThemeManager : IThemeManager, ITransientDependency, IServiceProviderAccessor
    {
        public IServiceProvider ServiceProvider { get; }

        public ITheme CurrentTheme => GetCurrentTheme();

        protected ThemingOptions Options { get; }

        public DefaultThemeManager(
            IOptions<ThemingOptions> options, 
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
        }

        protected virtual ITheme GetCurrentTheme()
        {
            var themeInfo = GetCurrentThemeInfo();
            return (ITheme) ServiceProvider.GetRequiredService(themeInfo.ThemeType);
        }

        protected virtual ThemeInfo GetCurrentThemeInfo()
        {
            if (!Options.Themes.Any())
            {
                throw new AbpException($"No theme registered! Use {nameof(ThemingOptions)} to register themes.");
            }

            if (Options.DefaultThemeName == null)
            {
                return Options.Themes.Values.First();
            }

            var themeInfo = Options.Themes.Values.FirstOrDefault(t => t.Name == Options.DefaultThemeName);
            if (themeInfo == null)
            {
                throw new AbpException("Default theme is configured but it's not found in the registered themes: " + Options.DefaultThemeName);
            }

            return themeInfo;
        }
    }
}