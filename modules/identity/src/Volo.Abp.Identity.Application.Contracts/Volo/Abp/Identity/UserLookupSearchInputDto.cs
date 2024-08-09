using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class UserLookupSearchInputDto : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
