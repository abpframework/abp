namespace Volo.Abp.Storage.Configuration
{
    public interface IAbpOptionError
    {
        string PropertyName { get; }

        string ErrorMessage { get; }
    }
}