using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy;

public interface IMultiTenantUrlProvider
{
    Task<string> GetUrlAsync(string templateUrl);
}