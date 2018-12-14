namespace Volo.Abp.Storage.FileSystem
{
    public interface IPublicUrlProvider
    {
        string GetPublicUrl(string storeName, FileSystemFileReference file);
    }
}
