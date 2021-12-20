using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;

namespace Volo.Abp.TextTemplating.Razor;

internal class NotFoundProjectItem : RazorProjectItem
{
    public NotFoundProjectItem(string basePath, string path, string fileKind)
    {
        BasePath = basePath;
        FilePath = path;
        FileKind = fileKind ?? FileKinds.GetFileKindFromFilePath(path);
    }

    public override string BasePath { get; }

    public override string FilePath { get; }

    public override string FileKind { get; }

    public override bool Exists => false;

    public override string PhysicalPath => throw new NotSupportedException();

    public override Stream Read() => throw new NotSupportedException();
}
