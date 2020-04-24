using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class SingleFileLocalizedTemplateContentReader : ILocalizedTemplateContentReader
    {
        private string _content;

        public async Task ReadContentsAsync(IFileInfo fileInfo)
        {
            _content = await fileInfo.ReadAsStringAsync();
        }

        public string GetContent(string culture, string defaultCultureName)
        {
            return _content;
        }
    }
}