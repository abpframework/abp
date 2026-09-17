namespace Volo.Abp.AspNetCore.Mvc.UI.Theming;

public class AbpThemingOptions
{
    public ThemeDictionary Themes { get; }

    public string? DefaultThemeName { get; set; }

    /// <summary>
    /// If set, the <c>base</c> element will be added to the <c>head</c> element of the page.
    /// eg: <base href="/BaseUrl/" />
    /// </summary>
    public string? BaseUrl { get; set; }

    public AbpThemingOptions()
    {
        Themes = new ThemeDictionary();
    }
}
