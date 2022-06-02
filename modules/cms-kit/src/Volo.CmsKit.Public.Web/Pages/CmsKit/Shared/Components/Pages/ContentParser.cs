using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Pages;

public class ContentParser : IContentParser, ITransientDependency
{
    public async Task<List<ContentFragment>> ParseAsync(string content)
    {
        return new List<ContentFragment> {
            new MarkdownContentFragment {
                Content = "**ABP Framework** is completely open source and developed in a community-driven manner."
            },
            new WidgetContentFragment("CmsPoll") { Properties = { { "widgetName", "poll-right" } } },
            new WidgetContentFragment("CmsPollByName") { Properties = { { "name", "poll-name" } } },
            new MarkdownContentFragment {
                Content = "Thanks _for_ *your* feedback."
            }
        };
    }
}