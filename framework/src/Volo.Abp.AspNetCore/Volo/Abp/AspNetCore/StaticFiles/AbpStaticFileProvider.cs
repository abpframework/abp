using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Primitives;

namespace Volo.Abp.AspNetCore.StaticFiles;

public class AbpStaticFileProvider : IFileProvider
{
    private readonly Matcher _matcher;
    private readonly IFileProvider _fileProvider;

    /// <param name="fileProvider">The file provider to be used to get the files.</param>
    /// <param name="fileNamePatterns">The file name patterns to include when serving static files (e.g., "appsettings*.json").
    /// Supports glob patterns. See <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing">Glob patterns documentation</see>.
    /// </param>
    public AbpStaticFileProvider(IReadOnlyList<string> fileNamePatterns, IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
        _matcher = new Matcher(StringComparison.OrdinalIgnoreCase);
        _matcher.AddIncludePatterns(fileNamePatterns);
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return new NotFoundDirectoryContents();
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        return _matcher.Match(Path.GetFileName(subpath)).HasMatches ?
            _fileProvider.GetFileInfo(subpath) :
            new NotFoundFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}