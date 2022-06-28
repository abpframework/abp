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
            _options.AddWidget("Poll", "CmsPollByCode", new List<PropertyDto>()
            {
                new PropertyDto() { Key = "Code", Name = "Code" }
            });

            _options.AddWidget("Comment", "CmsCommenting", new List<PropertyDto>()
            {
                new PropertyDto() { Key = "EntityType", Name = "EntityType" },
                new PropertyDto() { Key = "EntityId", Name = "EntityId" }
            });
        }
        return Task.FromResult(new ListResultDto<ContentWidgetDto>()
        {
            Items = _options.WidgetConfigs
                .Select(n =>
                    new ContentWidgetDto
                    {
                        Key = n.Key,
                        Properties = n.Value.Properties

                    }).ToList()
        });
    }
}
