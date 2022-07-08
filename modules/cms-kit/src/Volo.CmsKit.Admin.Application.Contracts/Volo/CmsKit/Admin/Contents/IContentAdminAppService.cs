using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Contents;
public interface IContentAdminAppService : IApplicationService
{
    Task<ListResultDto<ContentWidgetDto>> GetWidgetsAsync();
}
