using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class GetIdentityUsersInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
