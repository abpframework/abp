namespace Volo.Abp.BlobStoring.Google;

public interface IGoogleBlobNameCalculator
{
    string Calculate(BlobProviderArgs args);
}