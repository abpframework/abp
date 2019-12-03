using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;

namespace Volo.Docs.HtmlConverting
{
    public class DocumentSectionFinder : IDocumentSectionFinder
    {
        public ILogger<DocumentSectionFinder> Logger { get; set; }

        public DocumentSectionFinder()
        {
            Logger = NullLogger<DocumentSectionFinder>.Instance;
        }

        public DocumentSectionDictionary Find(string document)
        {
            return FindNextSections(document, new DocumentSectionDictionary());
            try
            {
                return FindNextSections(document, new DocumentSectionDictionary());
            }
            catch (Exception e)
            {
                Logger.LogWarning(
                    $"Incorrect usage of \"{DocumentSectionConsts.SectionOpenerPrefix}" +
                    $"KEY{DocumentSectionConsts.SectionOpenerKeyValueSeparator}" +
                    $"VALUE{DocumentSectionConsts.SectionOpenerPostfix}..{DocumentSectionConsts.SectionCloser}\" syntax.");
                return new DocumentSectionDictionary();
            }
        }

        private DocumentSectionDictionary FindNextSections(string targetPartOfDocument, DocumentSectionDictionary sections) 
        {
            var startOfSection = targetPartOfDocument.IndexOf(DocumentSectionConsts.SectionOpenerPrefix, StringComparison.InvariantCulture);

            if (startOfSection < 0)
            {
                return sections;
            }

            var endOfSectionOpener = targetPartOfDocument.Substring(startOfSection).IndexOf(DocumentSectionConsts.SectionOpenerPostfix, StringComparison.InvariantCulture);

            var keyValueWithSeparator = targetPartOfDocument.Substring(startOfSection)
                [DocumentSectionConsts.SectionOpenerPrefix.Length..endOfSectionOpener];

            var keyValue = keyValueWithSeparator.Split(DocumentSectionConsts.SectionOpenerKeyValueSeparator);
            var key = keyValue[0];
            var value = keyValue[1];

            if (sections.ContainsKey(key))
            {
                sections[key].AddIfNotContains(value);
            }
            else
            {
                sections.Add(key, new List<string>() { value });
            }

            var endOfSection = targetPartOfDocument.IndexOf(DocumentSectionConsts.SectionCloser, StringComparison.InvariantCulture)
                + DocumentSectionConsts.SectionCloser.Length;

            return FindNextSections(targetPartOfDocument.Substring(endOfSection), sections);
        }
    }
}
