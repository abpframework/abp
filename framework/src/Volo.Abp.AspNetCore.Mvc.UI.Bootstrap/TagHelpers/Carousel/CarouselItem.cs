namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Carousel;

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
