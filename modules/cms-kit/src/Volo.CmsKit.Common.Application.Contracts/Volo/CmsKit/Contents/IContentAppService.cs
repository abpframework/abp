using System.Threading.Tasks;

namespace Volo.CmsKit.Contents
{
    public interface IContentAppService
    {
        Task<ContentDto> GetAsync(GetContentInput input);
    }
}
