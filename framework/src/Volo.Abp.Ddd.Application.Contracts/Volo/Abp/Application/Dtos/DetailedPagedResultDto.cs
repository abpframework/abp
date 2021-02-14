using System;
using System.Collections.Generic;

namespace Volo.Abp.Application.Dtos
{
    [Serializable]
    public class DetailedPagedResultDto<T> : PagedResultDto<T>
    {
        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Last page count
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Number of items on the page
        /// </summary>
        public int PageSize => Items.Count;

        /// <summary>
        /// First element count in current page
        /// </summary>
        public long FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

        /// <summary>
        /// Last element count in current page
        /// </summary>
        public long LastRowOnPage => Math.Min(CurrentPage * PageSize, TotalCount);

        public DetailedPagedResultDto() { }

        public DetailedPagedResultDto(long totalCount, IReadOnlyList<T> items)
            : base(totalCount, items)
        {
            TotalCount = totalCount;
        }

        public DetailedPagedResultDto(int currentPage, int pageCount, long totalCount, IReadOnlyList<T> items)
         : base(totalCount, items)
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            TotalCount = totalCount;
        }
    }
}
