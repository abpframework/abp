using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles;

public class TemplateContentFileProvider : ITransientDependency
{
    protected IVirtualFileProvider VirtualFileProvider { get; }

    public TemplateContentFileProvider(IVirtualFileProvider virtualFileProvider)
    {
        VirtualFileProvider = virtualFileProvider;
    }

    public async Task<List<TemplateContentFileInfo>> GetFilesAsync(TemplateDefinition templateDefinition)
    {
        var files = new List<TemplateContentFileInfo>();

        var virtualPath = templateDefinition.GetVirtualFilePathOrNull();
        if (virtualPath == null)
        {
            return files;
        }

        var fileInfo = VirtualFileProvider.GetFileInfo(virtualPath);
        if (!fileInfo.Exists)
        {
            var directoryContents = VirtualFileProvider.GetDirectoryContents(virtualPath);
            if (!directoryContents.Exists)
            {
                throw new AbpException("Could not find a file/folder at the location: " + virtualPath);
            }

            fileInfo = new VirtualDirectoryFileInfo(virtualPath, virtualPath, DateTimeOffset.UtcNow);
        }

        if (fileInfo.IsDirectory)
        {
            //TODO: Configure file extensions.
            var folderReader = new VirtualFolderLocalizedTemplateContentReader(new[] { ".tpl", ".cshtml" });
            await folderReader.ReadContentsAsync(VirtualFileProvider, virtualPath);
            files.AddRange(folderReader.GetFiles());
        }
        else
        {
            var singleFileReader = new FileInfoLocalizedTemplateContentReader();
            await singleFileReader.ReadContentsAsync(fileInfo);
            files.Add(singleFileReader.GetFile());
        }

        return files;
    }
}
