using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Polls;
public class PollViewComponentManager : DomainService
{
    protected IContentParser ContentParser { get; set; }
    public PollViewComponentManager(IContentParser contentParser)
    {
        ContentParser = contentParser;
    }

    public virtual async Task<List<ContentFragment>> ParseAsync(string content)
    {
        return await ContentParser.ParseAsync(content);
    }

}
