using System.Threading.Tasks;
using Volo.CmsKit.Common.Application.Contracts.Volo.CmsKit.Contents;

namespace Volo.CmsKit.Contents
{
    public interface IContentAppService
    {
        Task<ContentDto> GetAsync(GetContentInput input);
    }
}
