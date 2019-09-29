using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DashboardDemo.Data
{
    /* This is used if database provider does't define
     * IDashboardDemoDbSchemaMigrator implementation.
     */
    public class NullDashboardDemoDbSchemaMigrator : IDashboardDemoDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}