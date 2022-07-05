using System.Collections.Generic;
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

    public virtual Task<ListResultDto<ContentWidgetDto>> GetWidgetsAsync()
    {
        //TODO remove
        if (!_options.WidgetConfigs.Any())
        {
            _options.AddWidget("CmsPollByCode", "Poll", "CmsPolls");
        }
        return Task.FromResult(new ListResultDto<ContentWidgetDto>()
        {
            Items = _options.WidgetConfigs
                .Select(n =>
                    new ContentWidgetDto
                    {
                        Key = n.Key,
                        Details = new WidgetDetailDto() { InternalName = n.Value.InternalName, Name = n.Value.Name },

                    }).ToList()
        });
    }
}
