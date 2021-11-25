using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace Volo.CmsKit;

public class FakeBlobProvider : IBlobProvider
{
    public virtual Task SaveAsync(BlobProviderSaveArgs args)
    {
        throw new System.NotImplementedException();
    }

    public virtual Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        throw new System.NotImplementedException();
    }

    public virtual Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        throw new System.NotImplementedException();
    }

    public virtual Task<Stream> GetAsync(BlobProviderGetArgs args)
    {
        throw new System.NotImplementedException();
    }

    public virtual Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
    {
        throw new System.NotImplementedException();
    }
}
