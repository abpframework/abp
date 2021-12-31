namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

public class PageItem
{
    public int Index { get; }

    public bool IsGap { get; set; }

    public PageItem(int index)
    {
        Index = index;
    }

    public PageItem(bool isGap)
    {
        IsGap = isGap;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is PageItem item))
        {
            return false;
        }

        return Index.Equals(item.Index);
    }

    public override int GetHashCode()
    {
        return Index.GetHashCode();
    }

    public int CompareTo(PageItem other)
    {
        if (Index > other.Index)
        {
            return 1;
        }

        if (Index < other.Index)
        {
            return -1;
        }

        return 0;
    }
}
