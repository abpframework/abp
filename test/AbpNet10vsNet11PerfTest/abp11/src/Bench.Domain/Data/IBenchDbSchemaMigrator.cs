using System.Threading.Tasks;

namespace Bench.Data;

public interface IBenchDbSchemaMigrator
{
    Task MigrateAsync();
}
