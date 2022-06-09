using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Public.Polls;

public interface IPollViewComponentAppService : IApplicationService
{
    Task<List<ContentFragment>> ParseAsync(string content);
}
