using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Azure
{
    public class AzureBlobProvider : BlobProviderBase, ITransientDependency
    {
        public override Task SaveAsync(BlobProviderSaveArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
