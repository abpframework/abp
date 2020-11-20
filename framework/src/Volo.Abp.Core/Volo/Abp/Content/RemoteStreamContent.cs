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

        public virtual string ContentType { get; set; }

        public virtual long? ContentLength => _stream.Length;

        public virtual Stream GetStream()
        {
            return _stream;
        }
    }
}
