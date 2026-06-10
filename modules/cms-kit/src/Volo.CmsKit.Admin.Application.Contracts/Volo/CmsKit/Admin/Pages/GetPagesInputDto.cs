using System;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin.Pages;

[Serializable]
public class GetPagesInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public PageStatus? Status { get; set; } = null;
}
