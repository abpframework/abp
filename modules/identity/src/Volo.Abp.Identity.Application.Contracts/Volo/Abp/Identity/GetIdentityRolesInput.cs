using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class GetIdentityRolesInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
