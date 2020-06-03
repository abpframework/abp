using System.Threading.Tasks;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp.EfCore
{
    public interface IBlobStoringHostDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}