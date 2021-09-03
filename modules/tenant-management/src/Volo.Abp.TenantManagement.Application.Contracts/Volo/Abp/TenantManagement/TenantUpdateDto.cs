using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TenantManagement
{
    public class TenantUpdateDto : TenantCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
}