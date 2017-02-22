using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.EmbeddedFiles;

namespace Volo.Abp.AspNetCore.EmbeddedFiles
{
    public class EmbeddedResourceItemFileInfo : IFileInfo
    {
        public bool Exists => true;

        public long Length => _fileInfo.Content.Length;

        public string PhysicalPath => null;

        public string Name => _fileInfo.FileName;

        public DateTimeOffset LastModified => _fileInfo.LastModifiedUtc;

        public bool IsDirectory => false;
        
        private readonly EmbeddedFileInfo _fileInfo;

        public EmbeddedResourceItemFileInfo(EmbeddedFileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public Stream CreateReadStream()
        {
            return new MemoryStream(_fileInfo.Content);
        }
    }
}