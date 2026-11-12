using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Bench.Data;

/* This is used if database provider does't define
 * IBenchDbSchemaMigrator implementation.
 */
public class NullBenchDbSchemaMigrator : IBenchDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
