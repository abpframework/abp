using System.IO;

namespace Volo.Abp.Content
{
    public class RemoteStreamContent : IRemoteStreamContent
    {
        private readonly Stream _stream;
        private readonly string _fileName;
        private readonly string _contentType;
        private readonly long? _length;
        private readonly bool _leaveOpen;

        public virtual string FileName => _fileName;
        public virtual string ContentType => _contentType;
        public virtual long? ContentLength => _length;

        public RemoteStreamContent(Stream stream, string fileName, string contentType = null, long? readOnlylength = null, bool leaveOpen = false)
        {
            _stream = stream;
            _fileName = fileName;
            _contentType = contentType ?? "application/octet-stream";
            _length = readOnlylength ?? (stream.GetNullableLength() - stream.GetNullablePosition());
            _leaveOpen = leaveOpen;
        }

        public virtual Stream GetStream()
        {
            return _stream;
        }

        public virtual void Dispose()
        {
            if (!_leaveOpen)
            {
                _stream?.Dispose();
            }
        }
    }
}
