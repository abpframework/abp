namespace Volo.Abp.Storage.Internal
{
    public class PrivateBlobReference : IPrivateBlobReference
    {
        public PrivateBlobReference(string path)
        {
            Path = path.Replace("\\", "/").TrimStart('/');
        }

        public string Path { get; }
    }
}