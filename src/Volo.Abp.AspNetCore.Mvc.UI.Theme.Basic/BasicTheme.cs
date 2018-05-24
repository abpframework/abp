using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic
{
    public class BasicTheme : ITheme, ITransientDependency
    {
        public string DefaultLayout => "~/Views/Shared/_AppLayout.cshtml";

        public string GetLayoutOrNull(string name)
        {
            return DefaultLayout;
        }
    }
}
