
namespace Volo.Abp.BlobStoring.HuaweiCloud
{
    public interface IHuaweiCloudBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}