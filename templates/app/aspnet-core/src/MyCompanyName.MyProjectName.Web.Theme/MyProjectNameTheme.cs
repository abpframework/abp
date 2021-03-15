using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName.Web.Theme
{
    [ThemeName(Name)]
    public class MyProjectNameTheme : ITheme, ITransientDependency
    {
        public const string Name = "MyProjectName";

        public virtual string GetLayout(string name, bool fallbackToDefault = true)
        {
            switch (name)
            {
                case StandardLayouts.Application:
                    return "~/Themes/MyProjectName/Layouts/Application.cshtml";
                case StandardLayouts.Account:
                    return "~/Themes/MyProjectName/Layouts/Account.cshtml";
                case StandardLayouts.Empty:
                    return "~/Themes/MyProjectName/Layouts/Empty.cshtml";
                default:
                    return fallbackToDefault ? "~/Themes/MyProjectName/Layouts/Application.cshtml" : null;
            }
        }
    }
}
