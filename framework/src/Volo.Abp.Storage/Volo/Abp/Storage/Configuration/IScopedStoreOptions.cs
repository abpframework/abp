namespace Volo.Abp.Storage.Configuration
{
    public interface IScopedStoreOptions : IAbpStoreOptions
    {
        string FolderNameFormat { get; }
    }
}