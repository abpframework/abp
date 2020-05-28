using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.BlobStoring.Database
{
    public class BlobStoringDatabaseDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;

        public BlobStoringDatabaseDataSeedContributor(
            IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }
        
        public Task SeedAsync(DataSeedContext context)
        {
            /* Instead of returning the Task.CompletedTask, you can insert your test data
             * at this point!
             */

            return Task.CompletedTask;
        }
    }
}