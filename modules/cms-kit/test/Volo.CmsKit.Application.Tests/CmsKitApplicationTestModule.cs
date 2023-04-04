using System.Collections.Generic;
using Volo.Abp.Modularity;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitApplicationModule),
    typeof(CmsKitDomainTestModule)
    )]
public class CmsKitApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<CmsKitCommentOptions>(options =>
        {
            options.AllowedExternalUrls = new List<string> { "https://abp.io" };
        });
    }
}
