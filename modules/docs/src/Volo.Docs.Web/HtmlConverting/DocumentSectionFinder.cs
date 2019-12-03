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

            var keysValues = targetPartOfDocument.Substring(startOfSection)
                [DocumentSectionConsts.SectionOpenerPrefix.Length..endOfSectionOpener];

            sections = ParseAndAddContidionToSections(keysValues, sections);

            return FindNextSections(targetPartOfDocument.Substring(endOfSectionOpener), sections);
        }

        private DocumentSectionDictionary ParseAndAddContidionToSections(string keysValues, DocumentSectionDictionary sections) 
        {
            if (keysValues.Contains(DocumentSectionConsts.SectionOpenerAndCondition, StringComparison.InvariantCulture))
            {
                var keysValuesSplitted = keysValues.Split(DocumentSectionConsts.SectionOpenerAndCondition);
                foreach (var keyValue in keysValuesSplitted)
                {
                    sections = AddContidionToSections(keyValue, sections);
                }
            }
            else if (keysValues.Contains(DocumentSectionConsts.SectionOpenerOrCondition, StringComparison.InvariantCulture))
            {
                var keysValuesSplitted = keysValues.Split(DocumentSectionConsts.SectionOpenerOrCondition);
                foreach (var keyValue in keysValuesSplitted)
                {
                    sections = AddContidionToSections(keyValue, sections);
                }
            }
            else
            {
                sections = AddContidionToSections(keysValues, sections);
            }

            return sections;
        }

        private DocumentSectionDictionary AddContidionToSections(string keyValueWithSeparator, DocumentSectionDictionary sections) 
        {
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

            return sections;
        }
    }
}
