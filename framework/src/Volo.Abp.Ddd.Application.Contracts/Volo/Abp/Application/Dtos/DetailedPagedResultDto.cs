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
        /// Number of items requested
        /// </summary>
        public int PageSize { get; set; }

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

        }

        public DetailedPagedResultDto(int currentPage, int pageCount, int pageSize, long totalCount, IReadOnlyList<T> items)
         : base(totalCount, items)
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            PageSize = PageSize;
        }
    }
}
