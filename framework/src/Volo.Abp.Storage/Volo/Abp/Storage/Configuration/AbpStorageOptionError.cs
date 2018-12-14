namespace Volo.Abp.Storage.Configuration
{
    public class AbpStorageOptionError : IAbpStorageOptionError
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }
    }
}
