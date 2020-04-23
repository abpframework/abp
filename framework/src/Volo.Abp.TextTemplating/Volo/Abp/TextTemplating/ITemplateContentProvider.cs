using System.Threading.Tasks;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateContentProvider
    {
        Task<string> GetContentOrNullAsync(string templateName, string cultureName);
    }
}