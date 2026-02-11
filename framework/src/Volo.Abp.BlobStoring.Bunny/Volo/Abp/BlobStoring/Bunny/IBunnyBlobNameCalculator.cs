namespace Volo.Abp.BlobStoring.Bunny;

public interface IBunnyBlobNameCalculator
{
    string Calculate(BlobProviderArgs args);
}
