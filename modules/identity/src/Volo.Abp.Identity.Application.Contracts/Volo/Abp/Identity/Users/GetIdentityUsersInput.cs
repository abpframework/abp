using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class GetIdentityUsersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
