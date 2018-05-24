using Volo.Abp.Collections;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theming
{
    public class ThemingOptions
    {
        public TypeList<ITheme> Themes { get; }

        public ThemingOptions()
        {
            Themes = new TypeList<ITheme>();
        }
    }
}
