using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Admin.Contents;

namespace Volo.CmsKit.Admin.Contents
{
    public interface IContentAdminAppService 
        : ICrudAppService<
            ContentDto,
            ContentGetListDto,
            Guid,
            ContentGetListInput,
            ContentCreateDto,
            ContentUpdateDto>
    {
        Task<ContentDto> GetAsync(string entityType, string entityId);
    }
}
