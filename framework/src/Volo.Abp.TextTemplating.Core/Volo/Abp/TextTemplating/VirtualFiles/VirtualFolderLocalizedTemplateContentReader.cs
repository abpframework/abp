using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles;

public class VirtualFolderLocalizedTemplateContentReader : ILocalizedTemplateContentReader
{
    private Dictionary<string, TemplateContentFileInfo> _dictionary = default!;
    private readonly string[] _fileExtension;

    public VirtualFolderLocalizedTemplateContentReader(string[] fileExtension)
    {
        _fileExtension = fileExtension;
    }

    public async Task ReadContentsAsync(
        IVirtualFileProvider virtualFileProvider,
        string virtualPath)
    {
        _dictionary = new Dictionary<string, TemplateContentFileInfo>();

        var directoryContents = virtualFileProvider.GetDirectoryContents(virtualPath);
        if (!directoryContents.Exists)
        {
            throw new AbpException("Could not find a folder at the location: " + virtualPath);
        }

        foreach (var file in directoryContents)
        {
            if (file.IsDirectory)
            {
                continue;
            }

            _dictionary.Add(file.Name.RemovePostFix(_fileExtension), new TemplateContentFileInfo()
            {
                FileName = file.Name,
                FileContent = await file.ReadAsStringAsync()
            });
        }
    }

    public string? GetContentOrNull(string? cultureName)
    {
        if (cultureName == null)
        {
            return null;
        }

        return _dictionary.GetOrDefault(cultureName)?.FileContent;
    }

    public List<TemplateContentFileInfo> GetFiles()
    {
        return _dictionary.Values.ToList();
    }
}
