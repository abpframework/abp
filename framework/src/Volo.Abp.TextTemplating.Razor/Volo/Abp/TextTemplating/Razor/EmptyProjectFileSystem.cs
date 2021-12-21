using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;

namespace Volo.Abp.TextTemplating.Razor;

internal class EmptyProjectFileSystem : RazorProjectFileSystem
{
    internal static readonly RazorProjectFileSystem Empty = new EmptyProjectFileSystem();

    public override IEnumerable<RazorProjectItem> EnumerateItems(string basePath)
    {
        NormalizeAndEnsureValidPath(basePath);
        return Enumerable.Empty<RazorProjectItem>();
    }

    [Obsolete("Use GetItem(string path, string fileKind) instead.")]
    public override RazorProjectItem GetItem(string path)
    {
        return GetItem(path, fileKind: null);
    }

    public override RazorProjectItem GetItem(string path, string fileKind)
    {
        NormalizeAndEnsureValidPath(path);
        return new NotFoundProjectItem(string.Empty, path, fileKind);
    }
}
