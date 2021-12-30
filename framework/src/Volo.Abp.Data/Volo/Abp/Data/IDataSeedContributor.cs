using System.Threading.Tasks;

namespace Volo.Abp.Data;

public interface IDataSeedContributor
{
    Task SeedAsync(DataSeedContext context);
}
