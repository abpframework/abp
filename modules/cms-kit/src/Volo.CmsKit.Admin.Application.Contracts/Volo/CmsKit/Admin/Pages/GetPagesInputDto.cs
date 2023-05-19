using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Pages;

[Serializable]
public class GetPagesInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
