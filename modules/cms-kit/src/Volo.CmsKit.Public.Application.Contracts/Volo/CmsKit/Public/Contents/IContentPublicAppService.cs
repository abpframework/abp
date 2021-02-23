using System.Threading.Tasks;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Public.Contents
{
    public interface IContentPublicAppService
    {
        Task<ContentDto> GetAsync(GetContentInput input);
    }
}