using System.Threading.Tasks;

namespace DashboardDemo.Data
{
    public interface IDashboardDemoDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
