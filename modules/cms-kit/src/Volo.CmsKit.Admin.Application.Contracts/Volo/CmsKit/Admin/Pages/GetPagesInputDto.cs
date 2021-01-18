using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Pages
{
    public class GetPagesInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}