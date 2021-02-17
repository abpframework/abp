using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// Simply inherit <see cref="IPagedResultRequestByPage"/>.
    /// </summary>
    [Serializable]
    public class PagedResultRequestByPageDto : LimitedResultRequestDto, IPagedResultRequestByPage
    {
        [Range(1, int.MaxValue)]
        public virtual int Page { get; set; } = 1;
    }
}
