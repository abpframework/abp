using System.Threading.Tasks;

namespace Volo.Abp.Data
{
    public interface IDataSeeder
    {
        Task SeedAsync(DataSeedContext context);
    }
}