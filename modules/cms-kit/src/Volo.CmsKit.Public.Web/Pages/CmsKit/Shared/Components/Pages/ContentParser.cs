using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Pages;

public class ContentParser : IContentParser, ITransientDependency
{
    private readonly CmsKitContentWidgetOptions _options;
    public ContentParser(IOptions<CmsKitContentWidgetOptions> options)
    {
        _options = options.Value;
    }

    public async Task<List<ContentFragment>> ParseAsync(string content)
    {
        var name = _options.WidgetConfigs.FirstOrDefault(p => p.Key == "Poll").Value?.Name;
        return new List<ContentFragment> {
            new MarkdownContentFragment {
                Content = "**ABP Framework** is completely open source and developed in a community-driven manner."
            },
            new WidgetContentFragment(name) {
                Properties = { 
                    { "name", "poll-name" }
                } 
            },
            new MarkdownContentFragment {
                Content = "Thanks _for_ *your* feedback."
            }
        };
    }
}