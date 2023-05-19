using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.TextTemplating.VirtualFiles;

public class FileInfoLocalizedTemplateContentReader : ILocalizedTemplateContentReader
{
    private string _content;

    public async Task ReadContentsAsync(IFileInfo fileInfo)
    {
        _content = await fileInfo.ReadAsStringAsync();
    }

    public string GetContentOrNull(string culture)
    {
        if (culture == null)
        {
            return _content;
        }

        return null;
    }
}
