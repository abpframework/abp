using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Contents
{
    public class ContentGetListInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
