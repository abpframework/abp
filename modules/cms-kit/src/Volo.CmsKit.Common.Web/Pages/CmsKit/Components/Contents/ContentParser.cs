using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.Contents;
using static Volo.CmsKit.Contents.ContentConsts;

namespace Volo.CmsKit.Web.Contents;

public class ContentParser : ITransientDependency
{
    private readonly CmsKitContentWidgetOptions _options;

    public ContentParser(IOptions<CmsKitContentWidgetOptions> options)
    {
        _options = options.Value;
    }

    public virtual Task<List<ContentFragment>> ParseAsync(string content)
    {
        if (!_options.WidgetConfigs.Any() || content is null)
        {
            return Task.FromResult(new List<ContentFragment>
            {
                new ContentFragment { Type = Markdown }.SetProperty(Content, content ?? string.Empty),
            });
        }

        List<string> parsedList = new();
        ParseContent(content, parsedList);

        List<ContentFragment> contentFragments = new();
        FillContentFragment(content, parsedList, contentFragments);

        return Task.FromResult(contentFragments);
    }

    protected virtual void ParseContent(string content, List<string> parsedList)
    {
        var replacedText = Regex.Replace(content, @"\[Widget.*?\]", Delimeter);
        if (!replacedText.Contains(Delimeter))
        {
            parsedList.Add(replacedText);
        }

        while (replacedText.Contains(Delimeter))
        {
            //For parsing delimeter
            var index = replacedText.IndexOf(Delimeter);
            if (index != 0)
            {
                parsedList.Add(replacedText.Substring(0, index));
                replacedText = replacedText.Substring(index, replacedText.Length - index);
                index = 0;
            }

            parsedList.Add(replacedText.Substring(index, Delimeter.Length));
            replacedText = replacedText.Substring(Delimeter.Length, replacedText.Length - Delimeter.Length);

            //for parsing the other side
            index = replacedText.IndexOf(Delimeter);
            if (index != -1)
            {
                parsedList.Add(replacedText.Substring(0, index));
                replacedText = replacedText.Substring(index, replacedText.Length - index);
            }
            else if (replacedText.Length > 0)
            {
                parsedList.Add(replacedText);
            }
        }
    }

    protected virtual void FillContentFragment(string content, List<string> parsedList, List<ContentFragment> contentFragments)
    {
        content = Regex.Replace(content, @"=\s*""", @"=""");
        content = Regex.Replace(content, @"""\s*=", @"""=");
        var widgets = Regex.Matches(content, @"(?<=\[Widget)(.*?)(?=\])").Cast<Match>().Select(p => p.Value).ToList();
        for (int i = 0, k = 0; i < parsedList.Count; i++)
        {
            if (parsedList[i] == Delimeter)
            {
                if (widgets.Count > k)
                {
                    var preparedContent = string.Join("", widgets[k]);
                    var keys = Regex.Matches(preparedContent, @"(?<=\s)(.*?)(?==\s*"")").Cast<Match>()
                        .Select(p => p.Value).Where(p => p != string.Empty).ToList();
                    var values = Regex.Matches(preparedContent, @"(?<=\s*[a-zA-Z]*=\s*"")(.*?)(?="")").Cast<Match>()
                        .Select(p => p.Value).ToList();

                    var widgetTypeIndex = keys.IndexOf("Type");
                    if (widgetTypeIndex != -1)
                    {
                        var widgetType = values[widgetTypeIndex];
                        var name = _options.WidgetConfigs.Where(p => p.Key == widgetType).Select(p => p.Value.Name).FirstOrDefault();
                        if (name is not null && widgets.Count > k)
                        {
                            values[0] = name;
                            var contentFragment = new ContentFragment() { Type = Widget };
                            contentFragments.Add(contentFragment);
                            for (int kv = 0; kv < values.Count; kv++)
                            {
                                contentFragments.FindLast(p => p == contentFragment)
                                    .SetProperty(keys[kv], values[kv]);
                            }
                        }
                    }
                }
                k++;
            }
            else
            {
                contentFragments.Add(new ContentFragment() { Type = Markdown }
                .SetProperty(Content, parsedList[i]));
            }
        }
    }
}