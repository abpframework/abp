using System.Threading.Tasks;

namespace MyCompanyName.MyProjectName.Data
{
    public interface IMyProjectNameDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
