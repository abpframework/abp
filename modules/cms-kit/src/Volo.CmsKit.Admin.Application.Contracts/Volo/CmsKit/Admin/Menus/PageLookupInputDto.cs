using System;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin.Menus;

[Serializable]
public class PageLookupInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public PageStatus? Status { get; set; }
}
