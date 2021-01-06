using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Tags
{
    public class TagGetListInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
