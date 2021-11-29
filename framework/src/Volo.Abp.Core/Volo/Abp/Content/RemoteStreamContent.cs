using System.IO;

namespace Volo.Abp.Content
{
    public class RemoteStreamContent : IRemoteStreamContent
    {
        private readonly Stream _stream;
        private readonly bool _disposeStream;
        private bool _disposed;

        public virtual string FileName { get; }

        public virtual string ContentType { get; } = "application/octet-stream";

        public virtual long? ContentLength { get; }

        public RemoteStreamContent(Stream stream, string fileName = null, string contentType = null, long? readOnlyLength = null, bool disposeStream = true)
        {
            _stream = stream;

            FileName = fileName;
            if (contentType != null)
            {
                ContentType = contentType;
            }
            ContentLength = readOnlyLength ?? (_stream.CanSeek ? _stream.Length - _stream.Position : null);
            _disposeStream = disposeStream;
        }

        public virtual Stream GetStream()
        {
            return _stream;
        }

        public virtual void Dispose()
        {
            if (!_disposed && _disposeStream)
            {
                _disposed = true;
                _stream?.Dispose();
            }
        }
    }
}
