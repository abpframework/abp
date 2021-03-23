using System.Collections.Generic;

namespace Volo.Abp.Cli.ProjectBuilding.Files
{
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
        
        //TODO: Remove this method and switch to symbol usage
        public static void RemoveTemplateCodeIf(this FileEntry file, string condition)
        {
            RemoveByCondition(file, "IF", condition);
        }

        //TODO: Remove this method and switch to symbol usage
        public static void RemoveTemplateCodeIfNot(this FileEntry file, string condition)
        {
            RemoveByCondition(file, "IF-NOT", condition);
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

        //TODO: Remove this method and switch to symbol usage
        private static void RemoveByCondition(this FileEntry file, string conditionName, string condition)
        {
            RemoveMarkedTemplateCode(file, $"<TEMPLATE-REMOVE {conditionName}='{condition}'>");
        }

        //TODO: Remove this method and switch to symbol usage
        private static void RemoveMarkedTemplateCode(this FileEntry file, string beginMark)
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
                if (lines[i].Contains(beginMark))
                {
                    while (i < lines.Length && !lines[i].Contains("</TEMPLATE-REMOVE>"))
                    {
                        ++i;
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
                    //TODO: PARSE symbol and condition (if available) !!!
                    var symbol = "UI-Angular"; //extract from IF/IF-NOT or set as null
                    var condition = true; //IF: true / IF-NOT: false

                    if (symbol != null &&
                        condition != symbols.Contains(symbol))
                    {
                        continue;
                    }
                    
                    while (i < lines.Length && !lines[i].Contains("</TEMPLATE-REMOVE>"))
                    {
                        ++i;
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
    }
}
