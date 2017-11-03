using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem.Embedded
{
    /// <summary>
    /// Represents a file embedded in an assembly.
    /// </summary>
    public class EmbeddedResourceFileInfo : IFileInfo
    {
        public bool Exists => true;

        public long Length
        {
            get
            {
                if (!_length.HasValue)
                {
                    using (var stream = _assembly.GetManifestResourceStream(_resourcePath))
                    {
                        _length = stream.Length;
                    }
                }

                return _length.Value;
            }
        }
        private long? _length;

        public string PhysicalPath { get; }

        public string Name { get; }

        public ManifestResourceInfo ManifestResourceInfo { get; }

        /// <summary>
        /// The time, in UTC.
        /// </summary>
        public DateTimeOffset LastModified { get; }

        public bool IsDirectory { get; }

        private readonly Assembly _assembly;
        private readonly string _resourcePath;

        public EmbeddedResourceFileInfo(
            Assembly assembly,
            string resourcePath,
            string physicalPath,
            string name,
            DateTimeOffset lastModified,
            bool isDirectory)
        {
            _assembly = assembly;
            _resourcePath = resourcePath;

            PhysicalPath = physicalPath;
            Name = name;

            if (_resourcePath != null)
            {
                ManifestResourceInfo = _assembly.GetManifestResourceInfo(_resourcePath);
            }

            LastModified = lastModified;
            IsDirectory = isDirectory;
        }

        /// <inheritdoc />
        public Stream CreateReadStream()
        {

            var stream = _assembly.GetManifestResourceStream(_resourcePath);

            if (!_length.HasValue)
            {
                _length = stream.Length;
            }

            return stream;
        }

        public override string ToString()
        {
            return $"[EmbeddedResourceFileInfo] {Name} ({PhysicalPath})";
        }
    }
}