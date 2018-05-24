namespace Volo.Abp.AspNetCore.Mvc.UI.Theming
{
    public class ThemingOptions
    {
        public ThemeDictionary Themes { get; }

        public string DefaultThemeName { get; set; }

        public ThemingOptions()
        {
            Themes = new ThemeDictionary();
        }
    }
}
