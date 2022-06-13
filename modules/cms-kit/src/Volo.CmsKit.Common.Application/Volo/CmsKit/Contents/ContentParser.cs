using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Contents;

public class ContentParser : ITransientDependency
{
    private const string delimeter = "----";
    private readonly CmsKitContentWidgetOptions _options;

    public ContentParser(IOptions<CmsKitContentWidgetOptions> options)
    {
        _options = options.Value;
    }

    public async Task<List<ContentFragment>> ParseAsync(string content)
    {
        return new List<ContentFragment> {
            new ContentFragment() { Type = "Markdown" }.SetProperty("Content", "This is *a markdown* text."),
            new ContentFragment() { Type = "Widget" }.SetProperty("Type", "Poll").SetProperty("Code", "6dhah8dd"),
            new ContentFragment() { Type = "Markdown" }.SetProperty("Content", "This is *another markdown* text.")
        };

        /*
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
            else if (replacedText.Length > 0)
            {
                parsedList.Add(replacedText);
            }
        }


        Dictionary<string, List<KeyValuePair<string, string>>> parsedWidgets = new();
        ParseWidgets(content, parsedWidgets);

        var contentFragments = new List<ContentFragment>();

        for (int i = 0, k = 0; i < parsedList.Count; i++)
        {
            if (parsedList[i] == delimeter)
            {
                var values = parsedWidgets.GetOrDefault($"{k}.Widget").Select(p => p.Value).ToList();
                var keys = parsedWidgets.GetOrDefault($"{k}.Widget").Select(p => p.Key).ToList();

                if (parsedWidgets.Count > k)
                {
                    var name = _options.WidgetConfigs.Where(p => p.Key == values[0]).Select(p => p.Value.Name).FirstOrDefault();
                    if (name is not null && parsedWidgets.Count > k)
                    {
                        var properties = new Dictionary<string, object>();
                        for (int kv = 0; kv < values.Count; kv++)
                        {
                            properties.Add(keys[kv], values[kv]);
                        }

                        contentFragments.Add(new WidgetContentFragment(name)
                        {
                            Properties = properties
                        });
                    }
                }
                k++;
            }
            else
            {
                contentFragments.Add(new MarkdownContentFragment() { Content = parsedList[i] });
            }
        }

        return contentFragments;
        */
    }

    private void ParseWidgets(string content, Dictionary<string, List<KeyValuePair<string, string>>> parsedWidgets)
    {
        var widgets = Regex.Matches(content, @"(?<=\[Widget)(.*?)(?=\])").Cast<Match>().Select(p => p.Value).ToList();
        for (int p = 0; p < widgets.Count; p++)
        {
            var preparedContent = string.Join("", widgets[p]);
            var keys = Regex.Matches(preparedContent, @"(?<=[\[Widget]?\s)(.*?)(?=="")").Cast<Match>()
                .Select(p => p.Value).Where(p => p != string.Empty).ToList();
            var values = Regex.Matches(preparedContent, @"(?<=\s*[a-zA-Z]*=\s*"")(.*?)(?="")").Cast<Match>()
                .Select(p => p.Value).ToList();

            var list = new List<KeyValuePair<string, string>>();
            for (int kv = 0; kv < keys.Count; kv++)
            {
                list.Add(new KeyValuePair<string, string>(keys[kv], values[kv]));
            }

            parsedWidgets.Add($"{p}.Widget", list);
        }
    }
}