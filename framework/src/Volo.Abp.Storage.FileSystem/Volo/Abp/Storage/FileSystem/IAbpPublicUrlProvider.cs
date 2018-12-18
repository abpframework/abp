namespace Volo.Abp.Storage.FileSystem
{
    public interface IAbpPublicUrlProvider
    {
        string GetPublicUrl(string storeName, FileSystemFileReference file);
    }
}
