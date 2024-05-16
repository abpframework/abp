using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Comments;

public interface ICommentAdminAppService : IApplicationService
{
    Task<PagedResultDto<CommentWithAuthorDto>> GetListAsync(CommentGetListInput input);

    Task<CommentWithAuthorDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);
    Task UpdateApprovalStatusAsync(Guid id, CommentApprovalDto commentApprovalDto);

    Task SetSettings(SettingsDto settingsDto);
    Task<SettingsDto> GetSettings();

}
