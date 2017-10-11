using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace Volo.Abp.Identity
{
    public class GetIdentityUsersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
