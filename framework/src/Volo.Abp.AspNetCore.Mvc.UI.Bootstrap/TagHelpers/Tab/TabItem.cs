namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class TabItem
    {
        public TabItem(string header, string content, bool active)
        {
            Header = header;
            Content = content;
            Active = active;
        }

        public string Header { get; set; }

        public string Content { get; set; }

        public bool Active { get; set; }
    }
}