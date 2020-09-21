namespace Volo.Abp.BlobStoring.Database
{
    public interface IDatabaseBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}
