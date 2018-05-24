using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
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

        protected Lazy<List<ITheme>> Themes { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultThemeManager(
            IOptions<ThemingOptions> options, 
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            ServiceProvider = serviceProvider;
            Options = options.Value;
            Themes = new Lazy<List<ITheme>>(CreateThemeList, true);
        }

        protected virtual ITheme GetCurrentTheme()
        {
            return Themes.Value.First();
            ////TODO: Temporary code!

            //var currentTheme = _httpContextAccessor.HttpContext.Items["_AbpCurrentTheme"] as ITheme;
            //if (currentTheme != null)
            //{
            //    return currentTheme;
            //}

            //_httpContextAccessor.HttpContext.Items["_AbpCurrentTheme"] = currentTheme = RandomHelper.GetRandomOf(Themes.Value.ToArray());
            //return currentTheme;
        }

        protected virtual List<ITheme> CreateThemeList()
        {
            return Options.Themes.Select(t => (ITheme) ServiceProvider.GetRequiredService(t)).ToList();
        }
    }
}