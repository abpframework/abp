using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Layout
{
    public class BreadCrumb
    {
        public List<BreadCrumbItem> Items { get; }

        public BreadCrumb()
        {
            Items = new List<BreadCrumbItem>();
        }
    }
}