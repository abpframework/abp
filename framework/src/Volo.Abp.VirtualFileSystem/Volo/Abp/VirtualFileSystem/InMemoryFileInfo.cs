using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem
{
    public class InMemoryFileInfo : IFileInfo
    {
        public bool Exists => true;

        public long Length => _fileContent.Length;

        public string PhysicalPath { get; }

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public bool IsDirectory => false;

        private readonly byte[] _fileContent;

        public InMemoryFileInfo(byte[] fileContent, string physicalPath, string name)
        {
            PhysicalPath = physicalPath;
            Name = name;
            _fileContent = fileContent;
            LastModified = DateTimeOffset.Now;
        }

        public Stream CreateReadStream()
        {
            return new MemoryStream(_fileContent, false);
        }
    }
}
