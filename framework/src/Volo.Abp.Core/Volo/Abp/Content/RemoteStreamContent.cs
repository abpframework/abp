using System.IO;

namespace Volo.Abp.Content
{
    public class RemoteStreamContent : IRemoteStreamContent
    {
        private readonly Stream _stream;

        public RemoteStreamContent(Stream stream)
        {
            _stream = stream;
        }

        public RemoteStreamContent(Stream stream, string fileName): this(stream)
        {
            FileName = fileName;
            ContentType = "application/octet-stream";
        }

        public virtual string ContentType { get; set; }

        public virtual long? ContentLength => _stream.Length;

        public virtual string FileName { get; set; }

        public virtual Stream GetStream()
        {
            return _stream;
        }
    }
}
