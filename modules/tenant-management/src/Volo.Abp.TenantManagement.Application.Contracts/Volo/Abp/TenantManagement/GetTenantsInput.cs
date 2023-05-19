using Volo.Abp.Application.Dtos;

namespace Volo.Abp.TenantManagement;

public class GetTenantsInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
