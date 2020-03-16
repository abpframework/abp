using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scriban;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Docs.Documents;

namespace Volo.Docs.HtmlConverting
{
    public class ScribanDocumentSectionRenderer : IDocumentSectionRenderer
    {
        private const string jsonOpener = "````json";
        private const string jsonCloser = "````";
        private const string docs_param = "//[doc-params]";
        private const string docs_templates = "//[doc-template]";

        public ILogger<ScribanDocumentSectionRenderer> Logger { get; set; }

        public ScribanDocumentSectionRenderer()
        {
            Logger = NullLogger<ScribanDocumentSectionRenderer>.Instance;
        }

        public async Task<string> RenderAsync(string document, DocumentRenderParameters parameters = null, List<DocumentPartialTemplateContent> partialTemplates = null)
        {
            if (partialTemplates != null && partialTemplates.Any())
            {
                document = SetPartialTemplates(document, partialTemplates);
            }

            var scribanTemplate = Template.Parse(document);

            if (parameters == null)
            {
                return await scribanTemplate.RenderAsync();
            }

            var result = await scribanTemplate.RenderAsync(parameters);
            return RemoveOptionsJson(result);
        }

        public async Task<Dictionary<string, List<string>>> GetAvailableParametersAsync(string document)
        {
            try
            {
                if (!document.Contains(jsonOpener) || !document.Contains(docs_param))
                {
                    return new Dictionary<string, List<string>>();
                }

                var (jsonBeginningIndex, jsonEndingIndex, insideJsonSection) = GetJsonBeginEndIndexesAndPureJson(document);

                if (jsonBeginningIndex < 0 || jsonEndingIndex <= 0 || string.IsNullOrWhiteSpace(insideJsonSection))
                {
                    return new Dictionary<string, List<string>>();
                }

                var pureJson = insideJsonSection.Replace(docs_param, "").Trim();

                return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(pureJson);
            }
            catch (Exception)
            {
                Logger.LogWarning("Unable to parse parameters of document.");
                return new Dictionary<string, List<string>>();
            }
        }

        private string RemoveOptionsJson(string document)
        {
            var orgDocument = document;
            try
            {
                if (!document.Contains(jsonOpener) || !document.Contains(docs_param))
                {
                    return orgDocument;
                }

                var (jsonBeginningIndex, jsonEndingIndex, insideJsonSection) = GetJsonBeginEndIndexesAndPureJson(document);

                if (jsonBeginningIndex < 0 || jsonEndingIndex <= 0 || string.IsNullOrWhiteSpace(insideJsonSection))
                {
                    return orgDocument;
                }

                return document.Remove(
                            jsonBeginningIndex - jsonOpener.Length, (jsonEndingIndex + jsonCloser.Length) - (jsonBeginningIndex - jsonOpener.Length)
                        );
            }
            catch (Exception)
            {
                return orgDocument;
            }
        }

        private (int, int, string) GetJsonBeginEndIndexesAndPureJson(string document)
        {
            var searchedIndex = 0;

            while (searchedIndex < document.Length)
            {
                var jsonBeginningIndex = document.Substring(searchedIndex).IndexOf(jsonOpener, StringComparison.Ordinal) + jsonOpener.Length + searchedIndex;

                if (jsonBeginningIndex < 0)
                {
                    return (-1, -1, "");
                }

                var jsonEndingIndex = document.Substring(jsonBeginningIndex).IndexOf(jsonCloser, StringComparison.Ordinal) + jsonBeginningIndex;
                var insideJsonSection = document[jsonBeginningIndex..jsonEndingIndex];

                if (insideJsonSection.IndexOf(docs_param) < 0)
                {
                    searchedIndex = jsonEndingIndex + jsonCloser.Length;
                    continue;
                }

                return (jsonBeginningIndex, jsonEndingIndex, insideJsonSection);
            }

            return (-1, -1, "");
        }

        public async Task<List<DocumentPartialTemplateWithValuesDto>> GetPartialTemplatesInDocumentAsync(string documentContent)
        {
            var templates = new List<DocumentPartialTemplateWithValuesDto>();

            while (documentContent.Contains(jsonOpener))
            {
                var afterJsonOpener = documentContent.Substring(
                    documentContent.IndexOf(jsonOpener, StringComparison.Ordinal) + jsonOpener.Length);

                var betweenJsonOpenerAndCloser = afterJsonOpener.Substring(0,
                    afterJsonOpener.IndexOf(jsonCloser, StringComparison.Ordinal));

                documentContent = afterJsonOpener.Substring(
                    afterJsonOpener.IndexOf(jsonCloser, StringComparison.Ordinal) + jsonCloser.Length);

                if (!betweenJsonOpenerAndCloser.Contains(docs_templates))
                {
                    continue;
                }

                var json = betweenJsonOpenerAndCloser.Substring(betweenJsonOpenerAndCloser.IndexOf(docs_templates, StringComparison.Ordinal) + docs_templates.Length);

                var template = JsonConvert.DeserializeObject<DocumentPartialTemplateWithValuesDto>(json);

                templates.Add(template);
            }

            return templates;
        }

        private string SetPartialTemplates(string document, List<DocumentPartialTemplateContent> templates)
        {
            var newDocument = new StringBuilder();

            while (document.Contains(jsonOpener))
            {
                var beforeJson = document.Substring(0,
                    document.IndexOf(jsonOpener, StringComparison.Ordinal) + jsonOpener.Length);

                var afterJsonOpener = document.Substring(
                    document.IndexOf(jsonOpener, StringComparison.Ordinal) + jsonOpener.Length);

                var betweenJsonOpenerAndCloser = afterJsonOpener.Substring(0,
                    afterJsonOpener.IndexOf(jsonCloser, StringComparison.Ordinal));

                if (!betweenJsonOpenerAndCloser.Contains(docs_templates))
                {
                    document = afterJsonOpener.Substring(
                        afterJsonOpener.IndexOf(jsonCloser, StringComparison.Ordinal) + jsonCloser.Length);
                    newDocument.Append(beforeJson + betweenJsonOpenerAndCloser + jsonCloser);
                    continue;
                }

                var json = betweenJsonOpenerAndCloser.Substring(betweenJsonOpenerAndCloser.IndexOf(docs_templates, StringComparison.Ordinal) + docs_templates.Length);

                var templatePath = JsonConvert.DeserializeObject<DocumentPartialTemplateWithValuesDto>(json)?.Path;

                var template = templates.FirstOrDefault(t => t.Path == templatePath);

                var beforeTemplate = document.Substring(0,
                    document.IndexOf(jsonOpener, StringComparison.Ordinal));

                newDocument.Append(beforeTemplate + template?.Content + jsonCloser);

                document = afterJsonOpener.Substring(
                    afterJsonOpener.IndexOf(jsonCloser, StringComparison.Ordinal) + jsonCloser.Length);
            }

            newDocument.Append(document);

            return newDocument.ToString();
        }
    }
}
