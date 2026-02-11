using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Public.Pages;

public interface IPagePublicAppService : IApplicationService
{
    Task<PageDto> FindBySlugAsync([NotNull] string slug);
    Task<bool> DoesSlugExistAsync([NotNull] string slug);
    Task<PageDto> FindDefaultHomePageAsync();
}
