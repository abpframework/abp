namespace Volo.Abp.AspNetCore.Mvc.UI.Theming
{
    public interface ITheme
    {
        string DefaultLayout { get; }

        string GetLayoutOrNull(string name);
    }
}