using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Blogs;

public class ContentParser : IContentParser, ITransientDependency
{
    private const string delimeter = "----";
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

        var replacedText = Regex.Replace(content, @"\[.*?\]", delimeter);

        var parsedList = new List<string>();

        if (!replacedText.Contains(delimeter))
        {
            parsedList.Add(replacedText);
        }

        while (replacedText.Contains(delimeter))
        {
            //For parsing delimeter
            var index = replacedText.IndexOf(delimeter);
            if (index != 0)
            {
                parsedList.Add(replacedText.Substring(0, index));
                replacedText = replacedText.Substring(index, replacedText.Length - index);
                index = 0;
            }

            parsedList.Add(replacedText.Substring(index, delimeter.Length));
            replacedText = replacedText.Substring(delimeter.Length, replacedText.Length - delimeter.Length);

            //for parsing the other side
            index = replacedText.IndexOf(delimeter);
            if (index != -1)
            {
                parsedList.Add(replacedText.Substring(0, index));
                replacedText = replacedText.Substring(index, replacedText.Length - index);
            }
            else
            {
                parsedList.Add(replacedText);
            }
        }

        var pollNames = Regex.Matches(content, @"(?<=PollName="")(.*?)(?="")").Select(p => p.Value).ToList();
        var polls = Regex.Matches(content, @"(?<=Widget Type="")(.*?)(?="")").Select(p => p.Value).ToList();

        var contentFragments = new List<ContentFragment>();

        for (int i = 0, k = 0; i < parsedList.Count; i++)
        {
            if (parsedList[i] == delimeter)
            {
                var name = _options.WidgetConfigs.Where(p => p.Key == polls[k]).Select(p => p.Value.Name).FirstOrDefault();
                if (name is not null)
                {
                    contentFragments.Add(new WidgetContentFragment(name)
                    {
                        Properties =
                        {
                            { "Name", pollNames[k]}
                        }
                    });
                }
                k++;
            }
            else
            {
                contentFragments.Add(new MarkdownContentFragment() { Content = parsedList[i] });
            }
        }

        return contentFragments;
    }
}