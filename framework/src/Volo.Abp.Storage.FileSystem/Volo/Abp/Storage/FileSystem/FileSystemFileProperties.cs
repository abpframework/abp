using System;
using System.Collections.Generic;
using System.IO;

namespace Volo.Abp.Storage.FileSystem
{
    public class FileSystemFileProperties : IFileProperties
    {
        private readonly FileInfo _fileInfo;
        private readonly FileExtendedProperties _extendedProperties;

        public FileSystemFileProperties(string fileSystemPath, FileExtendedProperties extendedProperties)
        {
            _fileInfo = new FileInfo(fileSystemPath);
            _extendedProperties = extendedProperties;
        }

        public DateTimeOffset? LastModified =>
            new DateTimeOffset(_fileInfo.LastWriteTimeUtc, TimeZoneInfo.Local.BaseUtcOffset);

        public long Length => _fileInfo.Length;

        public string ContentType
        {
            get => _extendedProperties.ContentType;
            set => _extendedProperties.ContentType = value;
        }

        public string ETag => _extendedProperties.ETag;

        public string CacheControl
        {
            get => _extendedProperties.CacheControl;
            set => _extendedProperties.CacheControl = value;
        }

        public string ContentMd5 => _extendedProperties.ContentMd5;

        public IDictionary<string, string> Metadata => _extendedProperties.Metadata;

        internal FileExtendedProperties ExtendedProperties => _extendedProperties;
    }
}