namespace Volo.Abp.AspNetCore.Mvc.UI.Theming
{
    public interface ITheme
    {
        string GetLayout(string name, bool fallbackToDefault = true);
    }
}