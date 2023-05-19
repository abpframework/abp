using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem;

public class InMemoryFileInfo : IFileInfo
{
    public bool Exists => true;

    public long Length => _fileContent.Length;

    public string PhysicalPath => null;

    public string Name { get; }

    public DateTimeOffset LastModified { get; }

    public bool IsDirectory => false;

    private readonly byte[] _fileContent;

    public string DynamicPath { get; }

    public InMemoryFileInfo(string dynamicPath, byte[] fileContent, string name)
    {
        DynamicPath = dynamicPath;
        Name = name;
        _fileContent = fileContent;
        LastModified = DateTimeOffset.Now;
    }

    public Stream CreateReadStream()
    {
        return new MemoryStream(_fileContent, false);
    }
}
