using System.Threading.Tasks;

namespace Volo.Abp.EntityFrameworkCore.ConnectionStrings;

public interface IAbpConnectionStringChecker
{
    Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString);
}
