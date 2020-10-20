namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic
{
    public partial class MainLayout
    {
        private bool IsCollapseShown { get; set; }

        private void ToggleCollapse()
        {
            IsCollapseShown = !IsCollapseShown;
        }
    }
}
