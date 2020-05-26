namespace Volo.Abp.BlobStoring.FileSystem
{
    public interface IBlogFilePathCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}