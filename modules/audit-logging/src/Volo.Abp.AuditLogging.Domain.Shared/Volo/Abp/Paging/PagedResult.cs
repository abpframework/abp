using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Paging
{
    public class PagedResult<T> : ListResultDto<T>, IPagedResult<T>
    {
        /// <inheritdoc />
        public long TotalCount { get; set; }

        /// <summary>
        /// Creates a new <see cref="PagedResult{T}"/> object.
        /// </summary>
        public PagedResult()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResult{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResult(long totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }

    public class ListResultDto<T> : IListResult<T>
    {
        /// <inheritdoc />
        public IReadOnlyList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
        private IReadOnlyList<T> _items;

        /// <summary>
        /// Creates a new <see cref="ListResultDto{T}"/> object.
        /// </summary>
        public ListResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ListResultDto{T}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }

    public interface IListResult<T>
    {
        IReadOnlyList<T> Items { get; set; }
    }

    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {
    }

    public interface IHasTotalCount
    {
        long TotalCount { get; set; }
    }
}
