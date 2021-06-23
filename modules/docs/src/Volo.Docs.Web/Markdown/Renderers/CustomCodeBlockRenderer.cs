using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Volo.Docs.Markdown.Renderers
{
    public class CustomCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
    {
        private const string Pattern = @"\{([^}]+)\}";
        public bool OutputAttributesOnPre { get; set; }

        public HashSet<string> BlocksAsDiv { get; }

        public CustomCodeBlockRenderer()
        {
            BlocksAsDiv = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
        
        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            renderer.EnsureLine();

            var fencedCodeBlock = obj as FencedCodeBlock;
            if (fencedCodeBlock?.Info != null && BlocksAsDiv.Contains(fencedCodeBlock.Info))
            {
                var infoPrefix = (obj.Parser as FencedCodeBlockParser)?.InfoPrefix ??
                                 FencedCodeBlockParser.DefaultInfoPrefix;

                if (renderer.EnableHtmlForBlock)
                {
                    renderer.Write("<div")
                        .WriteAttributes(obj.TryGetAttributes(),
                            cls => cls.StartsWith(infoPrefix, StringComparison.Ordinal) ? cls.Substring(infoPrefix.Length) : cls)
                        .Write('>');
                }

                renderer.WriteLeafRawLines(obj, true, true, true);

                if (renderer.EnableHtmlForBlock)
                {
                    renderer.WriteLine("</div>");
                }

            }
            else
            {
                if (renderer.EnableHtmlForBlock)
                {
                    renderer.Write("<pre ");

                    if (OutputAttributesOnPre)
                    {
                        renderer.WriteAttributes(obj);
                    }
                    
                    var highlightedLines = GetHighlightedLines(fencedCodeBlock);
                    if (highlightedLines.Any())
                    {
                        renderer.WriteAttributes(new HtmlAttributes {Classes = new List<string> {"line-numbers"}});

                        var lines = string.Join(",", highlightedLines);
                        renderer.Write($"data-line={lines}><code");
                    }
                    else
                    {
                        renderer.Write("><code");
                    }

                    if (!OutputAttributesOnPre)
                    {
                        renderer.WriteAttributes(obj);
                    }

                    renderer.Write('>');
                }

                renderer.WriteLeafRawLines(obj, true, true);

                if (renderer.EnableHtmlForBlock)
                {
                    renderer.WriteLine("</code></pre>");
                }
            }

            renderer.EnsureLine();
        }

        private List<int> GetHighlightedLines(FencedCodeBlock fencedCodeBlock)
        {
            var highlightedLines = new List<int>();

            if (string.IsNullOrWhiteSpace(fencedCodeBlock?.Arguments))
            {
                return highlightedLines;
            }

            if (Regex.IsMatch(pattern: Pattern, input: fencedCodeBlock.Arguments))
            {
                var match = Regex.Match(fencedCodeBlock.Arguments, Pattern);
                var groups = match.Groups;

                if (groups.Count < 2 || string.IsNullOrWhiteSpace(groups[1].Value))
                {
                    return highlightedLines;
                }
                
                var lines = groups[1].Value.Split(",");
                foreach (var line in lines)
                {
                    if (line.Contains("-"))
                    {
                        var numbers = line.Split("-");
                        highlightedLines.AddRange(numbers.Select(number => Convert.ToInt32(number)));
                    }
                    else
                    {
                        highlightedLines.Add(Convert.ToInt32(line));
                    }
                }
            }
            
            return highlightedLines;
        }
    }
}