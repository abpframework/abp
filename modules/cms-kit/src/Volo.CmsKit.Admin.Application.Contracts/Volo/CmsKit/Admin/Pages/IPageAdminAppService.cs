using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Pages;

public interface IPageAdminAppService : ICrudAppService<PageDto, PageDto, Guid, GetPagesInputDto, CreatePageInputDto, UpdatePageInputDto>
{
    Task SetAsHomePageAsync(Guid id);
}
