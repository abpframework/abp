using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Comments
{
    public interface ICommentAdminAppService : IApplicationService
    {
        Task<PagedResult<CommentDto>> GetListAsync(CommentGetListInput input);

        Task<CommentWithDetailsDto> GetAsync(string entityType, string entityId);

        Task DeleteAsync(string entityType, string entityId);
    }
}