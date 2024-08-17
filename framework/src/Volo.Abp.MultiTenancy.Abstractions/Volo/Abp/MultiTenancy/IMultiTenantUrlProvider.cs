using System.Threading.Tasks;

namespace Volo.Abp.MultiTenancy;

public interface IMultiTenantUrlProvider
{
    Task<string> GetUrlAsync(string templateUrl);
}