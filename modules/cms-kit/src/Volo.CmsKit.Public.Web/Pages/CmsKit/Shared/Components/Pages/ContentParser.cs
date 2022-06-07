using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        if (!_options.WidgetConfigs.Any())
        {
            return new List<ContentFragment>()
            {
                new MarkdownContentFragment() { Content = content }
            };
        }

        MatchCollection mc = Regex.Matches(content, @"(?<=\[Wid)(.*?)(?=\])");//(?<=X)(.*?)(?=Y)

        MatchCollection mcPollName = Regex.Matches(content, @"(?<=PollName="")(.*?)(?="")");
        var pollNames = mcPollName.Select(p => p.Value).ToList();

        string split = "-----";

        var polls = new List<string>();
        foreach (Match m in mc)
        {
            polls.Add("[Wid" + m + "]");
            content = content.Replace("[Wid" + m + "]", split);
        }

        var splittedContent = content.Split(split, StringSplitOptions.RemoveEmptyEntries);

        var contentFragments = new List<ContentFragment>();
        var name = _options.WidgetConfigs.FirstOrDefault(p => p.Key == "Poll").Value?.Name;
        for (int i = 0; i < splittedContent.Length; i++)
        {
            contentFragments.Add(new MarkdownContentFragment() { Content = splittedContent[i] });
            if (i != splittedContent.Length - 1)
            {
                contentFragments.Add(new WidgetContentFragment(name)
                {
                    Properties =
                    {
                        { "name", pollNames[i] }
                    }
                });
            }
        }

        return contentFragments;
    }
}