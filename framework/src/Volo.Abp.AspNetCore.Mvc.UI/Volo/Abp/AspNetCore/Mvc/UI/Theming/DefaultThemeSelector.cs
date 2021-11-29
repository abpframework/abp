using System.Linq;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theming
{
    public class DefaultThemeSelector : IThemeSelector, ITransientDependency
    {
        protected AbpThemingOptions Options { get; }

        public DefaultThemeSelector(IOptions<AbpThemingOptions> options)
        {
            Options = options.Value;
        }

        public virtual ThemeInfo GetCurrentThemeInfo()
        {
            if (!Options.Themes.Any())
            {
                throw new AbpException($"No theme registered! Use {nameof(AbpThemingOptions)} to register themes.");
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