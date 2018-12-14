namespace Volo.Abp.Storage.Configuration
{
    public interface IProviderInstanceOptions : INamedElementOptions
    {
        string Type { get; }
    }
}
