using System;
using System.Collections.Generic;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResultDto{T}.Items"/> list</typeparam>
    [Serializable]
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
    {
        /// <inheritdoc />
        public long TotalCount { get; set; } //TODO: Can be a long value..?

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        public PagedResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(long totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }

    /// <summary>
    /// Inherit <see cref="PagedResultDto{T}"/>.
    /// </summary>
    [Serializable]
    public class DetailedPagedResultDto<T> : PagedResultDto<T>, IDetailedPagedResult<T>
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
            PageSize = pageSize;
        }
    }
}