using System.Threading.Tasks;

namespace Volo.Abp.TenantManagement;

public interface ITenantValidator
{
    Task ValidateAsync(Tenant tenant);
}
