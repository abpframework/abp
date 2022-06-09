using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Public.Polls;

public class PollViewComponentAppService : CmsKitPublicAppServiceBase, IPollViewComponentAppService
{
    protected PollViewComponentManager PollViewComponentManager { get; set; }

    public PollViewComponentAppService(PollViewComponentManager pollViewComponentManager)
    {
        PollViewComponentManager = pollViewComponentManager;
    }

    public virtual async Task<List<ContentFragment>> ParseAsync(string content)
    {
        return await PollViewComponentManager.ParseAsync(content);
    }
}
