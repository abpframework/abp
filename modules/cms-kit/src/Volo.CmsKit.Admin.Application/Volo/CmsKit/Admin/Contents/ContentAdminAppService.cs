using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Admin.Contents;

public class ContentAdminAppService : CmsKitAdminAppServiceBase, IContentAdminAppService
{
    private readonly CmsKitContentWidgetOptions _options;

    public ContentAdminAppService(IOptions<CmsKitContentWidgetOptions> options)
    {
        _options = options.Value;
    }

    public Task<ListResultDto<ContentWidgetDto>> GetWidgetsAsync()
    {
        var aa = _options.WidgetConfigs.ToList();
        return Task.FromResult(new ListResultDto<ContentWidgetDto>()
        {
            Items = _options.WidgetConfigs
                .Select(n =>
                    new ContentWidgetDto
                    {
                        Key = n.Key

                    }).ToList()
        });
    }
}
