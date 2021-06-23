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
    public class HighlightedCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
    {
        private const string Pattern = @"\{([^}]+)\}";
        public bool OutputAttributesOnPre { get; set; }

        public HashSet<string> BlocksAsDiv { get; }

        public HighlightedCodeBlockRenderer()
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

                    WriteHighlightedCodeLines(renderer, fencedCodeBlock);

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
        
        private void WriteHighlightedCodeLines(HtmlRenderer renderer, FencedCodeBlock fencedCodeBlock)
        {
            var highlightedLines = GetHighlightedLines(fencedCodeBlock);
            if (highlightedLines.Any())
            {
                renderer.WriteAttributes(new HtmlAttributes
                {
                    Classes = new List<string> {"line-numbers"} //prevents adding line-numbers for highlighted lines
                });

                var lines = string.Join(",", highlightedLines);
                renderer.Write($"data-line={lines}><code");
            }
            else
            {
                renderer.Write("><code");
            }
        }
        
        private List<int> GetHighlightedLines(FencedCodeBlock fencedCodeBlock)
        {
            var highlightedLines = new List<int>();
            if (string.IsNullOrWhiteSpace(fencedCodeBlock?.Arguments) || !Regex.IsMatch(pattern: Pattern, input: fencedCodeBlock.Arguments))
            {
                return highlightedLines;
            }

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
                    if (numbers.Length > 2)
                    {
                        continue;
                    }

                    if (!int.TryParse(numbers[0], out var minLineNumber) ||
                        !int.TryParse(numbers[1], out var maxLineNumber))
                    {
                        continue;
                    }

                    for (var lineNumber = minLineNumber; lineNumber < maxLineNumber + 1; lineNumber++)
                    {
                        highlightedLines.Add(lineNumber);
                    }
                }
                else
                {
                    if (int.TryParse(line, out var lineNumber))
                    {
                        highlightedLines.Add(lineNumber);
                    }
                }
            }

            return highlightedLines;
        }
    }
}