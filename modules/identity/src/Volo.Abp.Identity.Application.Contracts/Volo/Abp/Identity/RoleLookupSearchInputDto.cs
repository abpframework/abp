using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class RoleLookupSearchInputDto : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
