using System;
using System.Linq;

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

        public virtual bool ShouldShowBreadCrumb()
        {
            if (BreadCrumb.Items.Any())
            {
                return true;
            }

            if (BreadCrumb.ShowCurrent && !Title.IsNullOrEmpty())
            {
                return true;
            }

            return false;
        }
    }
}