using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Database
{
    public abstract class DatabaseBlobContainer_Tests<TStartupModule> : BlobContainer_Tests<TStartupModule>
        where TStartupModule : IAbpModule
    {

    }
}
