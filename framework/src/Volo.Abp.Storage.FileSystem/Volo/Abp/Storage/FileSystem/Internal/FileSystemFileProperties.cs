using System;
using System.Collections.Generic;
using System.IO;

namespace Volo.Abp.Storage.FileSystem.Internal
{
    public class FileSystemFileProperties : IBlobDescriptor
    {
        private readonly FileInfo _fileInfo;

        public FileSystemFileProperties(string fileSystemPath, FileExtendedProperties extendedProperties)
        {
            _fileInfo = new FileInfo(fileSystemPath);
            ExtendedProperties = extendedProperties;
        }

        public string CacheControl
        {
            get => ExtendedProperties.CacheControl;
            set => ExtendedProperties.CacheControl = value;
        }

        internal FileExtendedProperties ExtendedProperties { get; }

        public DateTimeOffset? LastModified =>
            new DateTimeOffset(_fileInfo.LastWriteTimeUtc, TimeZoneInfo.Local.BaseUtcOffset);

        public long Length => _fileInfo.Length;

        public string ContentType
        {
            get => ExtendedProperties.ContentType;
            set => ExtendedProperties.ContentType = value;
        }

        public string ETag => ExtendedProperties.ETag;

        public string ContentMd5 => ExtendedProperties.ContentMd5;

        public IDictionary<string, string> Metadata => ExtendedProperties.Metadata;
    }
}