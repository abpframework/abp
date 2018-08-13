namespace Volo.Abp.Storage.Configuration
{
    public class AbpOptionError : IAbpOptionError
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }
    }
}