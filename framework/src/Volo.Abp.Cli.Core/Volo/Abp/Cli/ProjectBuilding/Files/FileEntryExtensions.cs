using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Files;

public static class FileEntryExtensions
{
    public static FileEntry ReplaceText(this FileEntry file, string oldText, string newText)
    {
        file.NormalizeLineEndings();
        file.SetContent(file.Content.Replace(oldText, newText));
        return file;
    }

    public static void RemoveTemplateCode(this FileEntry file, List<string> symbols)
    {
        RemoveMarkedTemplateCode(file, symbols);
    }

    public static void RemoveTemplateCodeMarkers(this FileEntry file)
    {
        if (!file.Content.Contains("</TEMPLATE-REMOVE>"))
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        var newLines = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("<TEMPLATE-REMOVE") ||
                lines[i].Contains("</TEMPLATE-REMOVE>"))
            {
                //TODO: What if we use inline like: <TEMPLATE-REMOVE IF-NOT="..."> some-code </TEMPLATE-REMOVE>
                //TODO: This logic skips the code in that case. Should handle it
                ++i;
            }

            if (i < lines.Length)
            {
                newLines.Add(lines[i]);
            }
        }

        file.SetLines(newLines);
    }

    private static void RemoveMarkedTemplateCode(this FileEntry file, List<string> symbols)
    {
        if (!file.Content.Contains("</TEMPLATE-REMOVE>"))
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        var newLines = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("<TEMPLATE-REMOVE"))
            {
                var parsedMarker = ParseTemplateRemoveMarker(lines[i]);

                bool sectionShouldBeRemoved = false;

                if (!parsedMarker.Symbols.Any())
                {
                    sectionShouldBeRemoved = true;
                }
                else if (parsedMarker.Symbols.Count == 1)
                {
                    sectionShouldBeRemoved = !parsedMarker.IsNegativeCondition
                        ? symbols.Contains(parsedMarker.Symbols[0], StringComparer.InvariantCultureIgnoreCase)
                        : !symbols.Contains(parsedMarker.Symbols[0], StringComparer.InvariantCultureIgnoreCase);
                }
                else if (parsedMarker.Operator == Operator.And)
                {
                    sectionShouldBeRemoved = parsedMarker.Symbols.Any(s => !parsedMarker.IsNegativeCondition
                        ? symbols.Contains(s, StringComparer.InvariantCultureIgnoreCase)
                        : !symbols.Contains(s, StringComparer.InvariantCultureIgnoreCase));
                }
                else if (parsedMarker.Operator == Operator.Or)
                {
                    sectionShouldBeRemoved = !parsedMarker.Symbols.All(s => !parsedMarker.IsNegativeCondition
                        ? symbols.Contains(s, StringComparer.InvariantCultureIgnoreCase)
                        : !symbols.Contains(s, StringComparer.InvariantCultureIgnoreCase) == false);
                }

                if (!sectionShouldBeRemoved)
                {
                    continue;
                }

                while (i < lines.Length && !lines[i].Contains("</TEMPLATE-REMOVE>"))
                {
                    ++i;
                }

                if (lines[i+1].Contains("<TEMPLATE-REMOVE"))
                {
                    continue;
                }
                
                ++i;
            }

            if (i < lines.Length)
            {
                newLines.Add(lines[i]);
            }
        }

        file.SetLines(newLines);
    }

    private static TemplateRemoveMarkerParseResult ParseTemplateRemoveMarker(string marker)
    {
        var result = new TemplateRemoveMarkerParseResult();

        var condition = marker.Trim()
            .RemovePreFix("//").Trim()
            .RemovePreFix("@*").Trim()
            .RemovePreFix("<!--").Trim()
            .RemovePreFix("<TEMPLATE-REMOVE").Trim()
            .RemovePostFix("*@").Trim()
            .RemovePostFix("-->").Trim()
            .RemovePostFix(">").Trim();

        if (string.IsNullOrWhiteSpace(condition))
        {
            return result;
        }

        var conditionSplitted = condition.Split("=");

        result.IsNegativeCondition = conditionSplitted[0] == "IF-NOT";

        var conditionContent = string.Join("=", conditionSplitted.Skip(1))
            .RemovePostFix("\"").RemovePreFix("\"")
            .RemovePostFix("'").RemovePreFix("'");

        if (conditionContent.Contains("&&"))
        {
            result.Operator = Operator.And;
            result.Symbols = conditionContent.Split("&&").ToList();
        }
        else if (conditionContent.Contains("||"))
        {
            result.Operator = Operator.Or;
            result.Symbols = conditionContent.Split("||").ToList();
        }
        else
        {
            result.Symbols.Add(conditionContent);
        }

        return result;
    }

    private class TemplateRemoveMarkerParseResult
    {
        public List<string> Symbols { get; set; } = new List<string>();

        public Operator Operator { get; set; } = Operator.None;

        public bool IsNegativeCondition { get; set; }
    }

    private enum Operator
    {
        None,
        And,
        Or
    }
}
