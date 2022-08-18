using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Contents;

[RequiresGlobalFeature(typeof(PagesFeature))]
public class ContentAppService : CmsKitAppServiceBase, IContentAppService
{
    protected ContentParser ContentParser { get; }

    public ContentAppService(ContentParser contentParser)
    {
        ContentParser = contentParser;
    }

    public async Task<List<ContentFragment>> ParseAsync(string content)
    {
        return await ContentParser.ParseAsync(content);
    }
}
