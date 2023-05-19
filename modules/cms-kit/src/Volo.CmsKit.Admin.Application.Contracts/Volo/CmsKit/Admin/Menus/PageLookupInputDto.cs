using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Menus;

[Serializable]
public class PageLookupInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
