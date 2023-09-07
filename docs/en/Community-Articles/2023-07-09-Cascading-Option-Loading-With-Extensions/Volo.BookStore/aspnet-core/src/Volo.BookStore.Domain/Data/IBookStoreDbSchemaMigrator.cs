using System.Threading.Tasks;

namespace Volo.BookStore.Data;

public interface IBookStoreDbSchemaMigrator
{
    Task MigrateAsync();
}
