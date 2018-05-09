using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Carousel
{
    public class CarouselItem
    {
        public CarouselItem(string html, bool active)
        {
            Html = html;
            Active = active;
        }

        public string Html { get; set; }

        public bool Active { get; set; }
    }
}
