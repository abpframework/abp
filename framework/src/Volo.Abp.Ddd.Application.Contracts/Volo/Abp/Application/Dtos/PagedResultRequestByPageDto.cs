using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// Simply inherit <see cref="PagedResultRequestDto"/>.
    /// </summary>
    [Serializable]
    public class PagedResultRequestByPageDto : PagedResultRequestDto
    {
        [Range(1, int.MaxValue)]
        public virtual int Page { get; set; }

        public override int SkipCount => (Page - 1) * MaxResultCount;
    }
}
