using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.CmsKit.Admin.Pages
{
    public interface IPageAdminAppService : ICrudAppService<PageDto, PageDto, Guid, GetPagesInputDto, CreatePageInputDto, UpdatePageInputDto>
    {
        Task<bool> ExistsAsync(string url);

        Task SetImageAsync(Guid id, RemoteStreamContent content);

        Task<RemoteStreamContent> GetImageAsync(Guid id);
    }
}