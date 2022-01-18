using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem;

public class VirtualDirectoryFileInfo : IFileInfo
{
    public bool Exists => true;

    public long Length => 0;

    public string PhysicalPath { get; }

    public string Name { get; }

    public DateTimeOffset LastModified { get; }

    public bool IsDirectory => true;

    public VirtualDirectoryFileInfo(string physicalPath, string name, DateTimeOffset lastModified)
    {
        PhysicalPath = physicalPath;
        Name = name;
        LastModified = lastModified;
    }

    public Stream CreateReadStream()
    {
        throw new InvalidOperationException();
    }
}
