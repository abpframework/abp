namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public interface IHuaweiyunBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}
