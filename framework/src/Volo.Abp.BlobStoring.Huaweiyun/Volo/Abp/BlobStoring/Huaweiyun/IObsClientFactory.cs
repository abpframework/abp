namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public interface IObsClientFactory
    {
        IObsClient Create(HuaweiyunBlobProviderConfiguration args);
    }
}
