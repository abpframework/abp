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
    // A widget ends at the first closing bracket and never spans lines, so matching stays linear.
    private const string WidgetPattern = @"\[Widget[^\]\r\n]*\bType\s*=[^\]\r\n]*\]";

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
        var replacedText = Regex.Replace(content, WidgetPattern, Delimeter, RegexOptions.NonBacktracking);
        if (!replacedText.Contains(Delimeter))
        {
            parsedList.Add(replacedText);
            return;
        }

        var parts = replacedText.Split(Delimeter);
        for (var i = 0; i < parts.Length; i++)
        {
            if (i > 0)
            {
                parsedList.Add(Delimeter);
            }

            if (parts[i].Length > 0 || (i > 0 && i < parts.Length - 1))
            {
                parsedList.Add(parts[i]);
            }
        }
    }

    protected virtual void FillContentFragment(string content, List<string> parsedList, List<ContentFragment> contentFragments)
    {
        var widgets = Regex.Matches(content, WidgetPattern, RegexOptions.NonBacktracking);
        for (int i = 0, k = 0; i < parsedList.Count; i++)
        {
            if (parsedList[i] == Delimeter)
            {
                if (k < widgets.Count)
                {
                    var properties = new Dictionary<string, string>();
                    foreach (Match attribute in Regex.Matches(widgets[k].Value, @"(?<key>[^\s=""\[\]]+)\s*=\s*""(?<value>[^""]*)""", RegexOptions.NonBacktracking))
                    {
                        properties[attribute.Groups["key"].Value] = attribute.Groups["value"].Value;
                    }

                    if (properties.TryGetValue("Type", out var widgetType) && _options.WidgetConfigs.TryGetValue(widgetType, out var config))
                    {
                        var fragment = new ContentFragment { Type = Widget };
                        foreach (var property in properties)
                        {
                            fragment.SetProperty(property.Key, property.Value);
                        }

                        fragment.SetProperty("Type", config.Name);
                        contentFragments.Add(fragment);
                    }
                }

                k++;
            }
            else
            {
                contentFragments.Add(new ContentFragment { Type = Markdown }.SetProperty(Content, parsedList[i]));
            }
        }
    }
}
