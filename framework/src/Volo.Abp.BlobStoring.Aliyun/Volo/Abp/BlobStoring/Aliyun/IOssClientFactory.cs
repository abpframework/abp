using Aliyun.OSS;

namespace Volo.Abp.BlobStoring.Aliyun;

public interface IOssClientFactory
{
    IOss Create(AliyunBlobProviderConfiguration args);
}
