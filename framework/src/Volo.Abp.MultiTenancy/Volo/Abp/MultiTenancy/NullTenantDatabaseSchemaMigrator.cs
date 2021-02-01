using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class NullTenantDatabaseSchemaMigrator : ITenantDatabaseSchemaMigrator, ISingletonDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}