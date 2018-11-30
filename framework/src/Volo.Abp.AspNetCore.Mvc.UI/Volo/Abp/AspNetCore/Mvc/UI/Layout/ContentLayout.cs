namespace Volo.Abp.AspNetCore.Mvc.UI.Layout
{
    public class ContentLayout
    {
        public string Title { get; set; }

        public BreadCrumb BreadCrumb { get; }

        public string MenuItemName { get; set; }

        public ContentLayout()
        {
            BreadCrumb = new BreadCrumb();
        }
    }
}