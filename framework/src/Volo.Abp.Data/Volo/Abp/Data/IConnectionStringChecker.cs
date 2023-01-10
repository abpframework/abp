using System.Threading.Tasks;

namespace Volo.Abp.Data;

public interface IConnectionStringChecker
{
    Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString);
}
