using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.AbpWebSite
{
    [ThemeName(Name)]
    public class AbpIoTheme : ITheme, ITransientDependency
    {
        public const string Name = "AbpIo";

        public string GetLayout(string name, bool fallbackToDefault = true)
        {
            switch (name)
            {
                case StandardLayouts.Empty:
                    return "~/Pages/Shared/LayoutEmpty.cshtml";
                default:
                    return fallbackToDefault ? "~/Pages/Shared/Layout.cshtml" : null;
            }
        }
    }
}
