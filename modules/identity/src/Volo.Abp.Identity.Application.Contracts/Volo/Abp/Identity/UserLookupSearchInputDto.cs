using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class UserLookupSearchInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}