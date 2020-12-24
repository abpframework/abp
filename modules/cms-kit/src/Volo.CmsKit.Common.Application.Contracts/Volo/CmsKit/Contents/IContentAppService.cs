using System.Threading.Tasks;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Contents
{
    public interface IContentAppService
    {
        Task<ContentDto> GetAsync(string entityType, string entityId);
    }
}
