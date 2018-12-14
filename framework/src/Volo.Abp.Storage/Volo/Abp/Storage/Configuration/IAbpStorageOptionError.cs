namespace Volo.Abp.Storage.Configuration
{
    public interface IAbpStorageOptionError
    {
        string PropertyName { get; }

        string ErrorMessage { get; }
    }
}
