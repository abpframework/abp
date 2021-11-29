namespace Volo.Abp.AspNetCore.Mvc.UI.Layout
{
    public class BreadCrumbItem
    {
        public string Text { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public BreadCrumbItem(string text, string url = null, string icon = null)
        {
            Text = text;
            Url = url;
            Icon = icon;
        }
    }
}