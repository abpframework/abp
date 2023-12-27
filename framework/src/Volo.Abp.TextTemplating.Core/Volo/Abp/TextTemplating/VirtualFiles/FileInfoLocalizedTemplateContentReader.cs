using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.TextTemplating.VirtualFiles;

public class FileInfoLocalizedTemplateContentReader : ILocalizedTemplateContentReader
{
    private IFileInfo _fileInfo = default!;
    private string _content = default!;

    public async Task ReadContentsAsync(IFileInfo fileInfo)
    {
        _fileInfo = fileInfo;
        _content = await fileInfo.ReadAsStringAsync();
    }

    public string? GetContentOrNull(string? culture)
    {
        if (culture == null)
        {
            return _content;
        }

        return null;
    }

    public TemplateContentFileInfo GetFile()
    {
        return new TemplateContentFileInfo()
        {
            FileName = _fileInfo.Name,
            FileContent = _content
        };
    }
}
