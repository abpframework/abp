using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Data;

public class DefaultConnectionStringChecker : IConnectionStringChecker, ITransientDependency
{
    public Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        return Task.FromResult(new AbpConnectionStringCheckResult
        {
            Connected = false,
            DatabaseExists = false
        });
    }
}
