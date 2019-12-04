using System;

namespace Volo.Docs.HtmlConverting
{
    public class DocumentSectionHtmlReplacer : IDocumentSectionHtmlReplacer
    {
        public string Replace(string document)
        {
            while (document.Contains(DocumentSectionConsts.SectionOpenerPrefix))
            {
                var sectionIndex = document.IndexOf(DocumentSectionConsts.SectionOpenerPrefix, StringComparison.InvariantCulture);

                var sectionCloserIndex = document.Substring(sectionIndex + DocumentSectionConsts.SectionOpenerPrefix.Length)
                    .IndexOf(DocumentSectionConsts.SectionOpenerPostfix, StringComparison.InvariantCulture);

                var keysValues = document.Substring(sectionIndex + DocumentSectionConsts.SectionOpenerPrefix.Length)
                [0..sectionCloserIndex];

                var andOperation = keysValues.Contains(DocumentSectionConsts.SectionOpenerAndCondition, StringComparison.InvariantCulture);
                var orOperation = keysValues.Contains(DocumentSectionConsts.SectionOpenerOrCondition, StringComparison.InvariantCulture);

                var splitterChar = andOperation ? DocumentSectionConsts.SectionOpenerAndCondition : DocumentSectionConsts.SectionOpenerOrCondition;

                string[] keysValuesSplitted = keysValues.Split(splitterChar);

                var keys = new string[keysValuesSplitted.Length];
                var values = new string[keysValuesSplitted.Length];

                for (int i = 0; i < keysValuesSplitted.Length; i++)
                {
                    keys[i] = keysValuesSplitted[i].Split(DocumentSectionConsts.SectionOpenerKeyValueSeparator)[0];
                    values[i] = keysValuesSplitted[i].Split(DocumentSectionConsts.SectionOpenerKeyValueSeparator)[1];
                }

                var div = $"<span class=\"doc-section\" data-keys=\"" +
                    $"{string.Join(splitterChar, keys)}" +
                    $"\" data-values=\"" +
                    $"{string.Join(splitterChar, values)}" +
                    $"\">";

                document = document.Remove(sectionIndex, sectionCloserIndex + DocumentSectionConsts.SectionOpenerPrefix.Length +1);

                document = document.Insert(sectionIndex, div);
            }

            document = document.Replace(DocumentSectionConsts.SectionCloser, "</span>");

            return document;
        }
    }
}
