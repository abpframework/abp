using Blazorise;

namespace Volo.Abp.BlazoriseUI
{
    public class BreadcrumbItem
    {
        public string Text { get; set; }

        public IconName? Icon { get; set; }

        public string Url { get; set; }

        public BreadcrumbItem(string text, string url = null, IconName? icon = null)
        {
            Text = text;
            Url = url;
            Icon = icon;
        }
    }
}
