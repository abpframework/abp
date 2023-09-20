namespace Volo.CmsKit.Pages;
public class PageCacheKey
{
    public PageCacheKey(string slug)
    {
        Slug = slug;
    }

    public string Slug { get; set; }

    public override string ToString()
    {
        return $"CmsPage_{Slug}";
    }
}