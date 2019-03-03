using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Layout
{
    public class BreadCrumb
    {
        /// <summary>
        /// Default: true.
        /// </summary>
        public bool ShowHome { get; set; } = true;

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool ShowCurrent { get; set; } = true;

        public List<BreadCrumbItem> Items { get; }

        public BreadCrumb()
        {
            Items = new List<BreadCrumbItem>();
        }

        public void Add(string text, string url = null, string icon = null)
        {
            Items.Add(new BreadCrumbItem(text, url, icon));
        }
    }
}