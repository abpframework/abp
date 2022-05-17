using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.DependencyInjection;
using Ganss.XSS;

namespace Volo.CmsKit.Public.Web.Renderers;

public class MarkdownToHtmlRenderer : IMarkdownToHtmlRenderer, ITransientDependency
{
    private readonly HtmlSanitizer _htmlSanitizer;
    protected MarkdownPipeline MarkdownPipeline { get; }

    public MarkdownToHtmlRenderer(MarkdownPipeline markdownPipeline)
    {
        MarkdownPipeline = markdownPipeline;
        _htmlSanitizer = new HtmlSanitizer();
    }

    public async Task<string> RenderAsync(string rawMarkdown, bool preventXSS = false)
    {
        if (preventXSS)
        {
            rawMarkdown = EncodeHtmlTags(rawMarkdown, true);
        }

        var html = Markdown.ToHtml(rawMarkdown, MarkdownPipeline);

        if (preventXSS)
        {
            html = _htmlSanitizer.Sanitize(html);
        }

        return html;
    }


    private static List<CodeBlockIndexPair> GetCodeBlockIndices(string markdownText)
    {
        var regexObj = new Regex(@"```(\w)*|`(\w)*", RegexOptions.IgnoreCase |
                                              RegexOptions.IgnorePatternWhitespace |
                                              RegexOptions.Singleline |
                                              RegexOptions.Multiline |
                                              RegexOptions.ExplicitCapture);

        var matches = regexObj.Matches(markdownText);
        var indices = new List<CodeBlockIndexPair>();

        for (var i = 0; i < matches.Count; i++)
        {
            if (!indices.Any() || indices.Last().EndIndex.HasValue)
            {
                indices.Add(new CodeBlockIndexPair(matches[i].Index));
            }
            else
            {
                indices.Last().EndIndex = matches[i].Index;
            }
        }

        return indices;
    }

    /// <summary>
    /// Encodes html tags.
    /// </summary>
    private static string EncodeHtmlTags(string text, bool dontEncodeCodeBlocks = true)
    {
        List<CodeBlockIndexPair> codeBlockIndices = null;
        if (dontEncodeCodeBlocks)
        {
            codeBlockIndices = GetCodeBlockIndices(text);
        }

        return Regex.Replace(text, @"<[^>]*>", match =>
        {
            if (dontEncodeCodeBlocks && codeBlockIndices != null)
            {
                var isInCodeBlock = false;
                foreach (var codeBlock in codeBlockIndices)
                {
                    if (IsInCodeBlock(match.Index, codeBlock.StartIndex, codeBlock.EndIndex))
                    {
                        isInCodeBlock = true;
                        break;
                    }
                }

                if (isInCodeBlock)
                {
                    return match.ToString();
                }
                else
                {
                    return HttpUtility.HtmlEncode(match.ToString());
                }
            }
            else
            {
                return HttpUtility.HtmlEncode(match.ToString());
            }
        });
    }

    private static bool IsInCodeBlock(int currentIndex, int codeBlockStartIndex, int? codeBlockEndIndex)
    {
        if (codeBlockEndIndex.HasValue)
        {
            return (currentIndex >= codeBlockStartIndex && currentIndex <= codeBlockEndIndex);
        }

        return currentIndex >= codeBlockStartIndex;
    }

    private class CodeBlockIndexPair
    {
        public int StartIndex { get; private set; }
        public int? EndIndex { get; set; }

        public CodeBlockIndexPair(int startIndex, int? endIndex = null)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}
