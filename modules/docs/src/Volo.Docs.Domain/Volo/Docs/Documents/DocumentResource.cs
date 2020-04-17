namespace Volo.Docs.Documents
{
    public class DocumentResource
    {
        public byte[] Content { get; }

        public DocumentResource(byte[] content)
        {
            Content = content;
        }
    }
}