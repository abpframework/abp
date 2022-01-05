using System;
using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Application.Dtos
{
    [Serializable]
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

    [Serializable]
    public class ExtensibleListResultDto<T> : ExtensibleObject, IListResult<T>
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
        public ExtensibleListResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ListResultDto{T}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ExtensibleListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }
}
