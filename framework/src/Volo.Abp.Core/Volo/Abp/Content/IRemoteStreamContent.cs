using System.IO;

namespace Volo.Abp.Content
{
    public interface IRemoteStreamContent
    {
        string ContentType { get; }

        long? ContentLength { get; }

        string FileName { get; }

        Stream GetStream();
    }
}
