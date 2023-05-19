namespace Volo.Abp.AspNetCore.Mvc.UI.Theming;

public class AbpThemingOptions
{
    public ThemeDictionary Themes { get; }

    public string DefaultThemeName { get; set; }

    public AbpThemingOptions()
    {
        Themes = new ThemeDictionary();
    }
}
