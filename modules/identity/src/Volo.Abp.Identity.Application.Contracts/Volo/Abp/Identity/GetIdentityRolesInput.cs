using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class GetIdentityRolesInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
