using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
            set
            {
                SkipCount = value;
            }
        }

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 20;

        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; } = 1;
    }
}