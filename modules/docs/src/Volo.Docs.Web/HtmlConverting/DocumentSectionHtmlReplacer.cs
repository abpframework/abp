namespace Volo.Docs.HtmlConverting
{
    public class DocumentSectionHtmlReplacer : IDocumentSectionHtmlReplacer
    {
        public string Replace(string document, DocumentSectionDictionary sections)
        {
            foreach (var key in sections)
            {
                foreach (var value in key.Value)
                {
                    document = document.Replace(
                        DocumentSectionConsts.SectionOpenerPrefix + key.Key + DocumentSectionConsts.SectionOpenerKeyValueSeparator + value + DocumentSectionConsts.SectionOpenerPostfix,
                        $"<div class=\"doc-section\" data-key=\"{key.Key}\" data-value=\"{value}\" style=\"display: none\">");
                }
            }

            document = document.Replace(DocumentSectionConsts.SectionCloser, "</div>");

            return document;
        }
    }
}
