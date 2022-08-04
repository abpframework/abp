using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Contents;

public interface IContentAppService : IApplicationService
{
    Task<List<ContentFragment>> ParseAsync(string content);
}
