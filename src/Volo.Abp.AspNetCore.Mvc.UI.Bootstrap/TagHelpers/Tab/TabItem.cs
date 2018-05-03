namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class TabItem
    {
        public TabItem(string header, string content)
        {
            Header = header;
            Content = content;
        }

        public string Header { get; set; }

        public string Content { get; set; }
    }
}