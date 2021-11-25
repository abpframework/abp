namespace Volo.Abp.BlobStoring.Minio;

public interface IMinioBlobNameCalculator
{
    string Calculate(BlobProviderArgs args);
}
