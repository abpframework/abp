namespace Volo.Abp.BlobStoring.Aliyun
{
    public interface IAliyunBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}
