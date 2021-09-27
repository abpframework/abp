using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Tags
{
    [Serializable]
    public class TagGetListInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
