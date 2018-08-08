namespace Volo.Abp.Storage
{
    public class StorageError
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string ProviderMessage { get; set; }
    }
}