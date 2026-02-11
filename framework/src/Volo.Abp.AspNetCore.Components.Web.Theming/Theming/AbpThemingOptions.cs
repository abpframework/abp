namespace Volo.Abp.AspNetCore.Components.Web.Theming.Theming;

public class AbpThemingOptions
{
    public ThemeDictionary Themes { get; }

    public string? DefaultThemeName { get; set; }

    public AbpThemingOptions()
    {
        Themes = new ThemeDictionary();
    }
}
