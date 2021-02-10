using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogGetListInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
