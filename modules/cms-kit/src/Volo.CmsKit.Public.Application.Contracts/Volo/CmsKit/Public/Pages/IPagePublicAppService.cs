using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Pages;

public interface IPagePublicAppService : IApplicationService
{
    Task<PageDto> FindBySlugAsync([NotNull] string slug);
}
