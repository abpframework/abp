namespace Volo.Abp.BlobStoring.Aws
{
    public interface IAwsBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}
