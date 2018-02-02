using Volo.Abp.Application.Dtos;

namespace Volo.Abp.MultiTenancy
{
    public class GetTenantsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}