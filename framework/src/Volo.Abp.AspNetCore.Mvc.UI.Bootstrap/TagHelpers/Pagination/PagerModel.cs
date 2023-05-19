using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

public class PagerModel
{
    public long TotalItemsCount { get; set; }

    public int ShownItemsCount { get; }

    public int PageSize { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPageCount { get; set; }

    public int ShowingFrom { get; set; }

    public int ShowingTo { get; set; }

    public List<PageItem> Pages { get; set; }

    public int PreviousPage { get; set; }

    public int NextPage { get; set; }

    public string Sort { get; set; }

    public string PageUrl { get; set; }

    private const int MaxItemsCountToShowAllPages = 4;

    public PagerModel(long totalCount, int shownItemsCount, int currentPage, int pageSize, string pageUrl, string sort = null)
    {
        TotalItemsCount = totalCount;
        ShownItemsCount = shownItemsCount;
        PageSize = pageSize;
        TotalPageCount = (int)Math.Ceiling(Convert.ToDouble((decimal)TotalItemsCount / PageSize));
        Sort = sort;

        PageUrl = pageUrl?.EnsureStartsWith('/') ?? "/";

        if (currentPage > TotalPageCount)
        {
            CurrentPage = TotalPageCount;
        }
        else if (currentPage < 1)
        {
            CurrentPage = 1;
        }
        else
        {
            CurrentPage = currentPage;
        }

        ShowingFrom = totalCount == 0 ? 0 : (CurrentPage - 1) * PageSize + 1;
        ShowingTo = totalCount == 0 ? 0 : (int)Math.Min(ShowingFrom + PageSize - 1, totalCount);
        PreviousPage = CurrentPage <= 1 ? 1 : CurrentPage - 1;
        NextPage = CurrentPage >= TotalPageCount ? CurrentPage : CurrentPage + 1;
        Pages = CalculatePageNumbers();
    }

    private List<PageItem> CalculatePageNumbers()
    {
        return TotalPageCount <= MaxItemsCountToShowAllPages ?
            GetAllPages() :
            GetPagesWithGaps();
    }

    /// <summary>
    /// Gets first two, previous, current, next, last two pages
    /// </summary>
    private List<PageItem> GetPagesWithGaps()
    {
        var pages = new List<PageItem>();
        var firstPage = new PageItem(1);
        var secondPage = new PageItem(2);
        var pageBeforeLastPage = new PageItem(TotalPageCount - 1);
        var lastPage = new PageItem(TotalPageCount);

        //first two pages
        pages.Add(firstPage);
        pages.Add(secondPage);

        //current page segment
        pages.AddIfNotContains(new PageItem(PreviousPage));
        pages.AddIfNotContains(new PageItem(CurrentPage));
        pages.AddIfNotContains(new PageItem(NextPage));

        //last two pages
        pages.AddIfNotContains(pageBeforeLastPage);
        pages.AddIfNotContains(lastPage);

        AddGaps(pages);
        return pages;
    }

    private List<PageItem> GetAllPages()
    {
        var pages = new List<PageItem>();
        for (var i = 1; i <= TotalPageCount; ++i)
        {
            pages.Add(new PageItem(i));
        }

        return pages;
    }

    private static void AddGaps(IList<PageItem> pages)
    {
        var pageCount = pages.Count;
        for (var i = 0; i < pageCount - 1; i++)
        {
            var current = pages[i].Index;
            var next = pages[i + 1].Index;

            if (current + 1 == next)
            {
                continue;
            }

            pages.Insert(i + 1, new PageItem(true));
            pageCount++;
            i++;
        }
    }
}
