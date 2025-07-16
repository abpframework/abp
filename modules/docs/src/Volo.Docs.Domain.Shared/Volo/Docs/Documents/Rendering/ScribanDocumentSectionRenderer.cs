using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Scriban;
using Volo.Abp;
using Volo.Extensions;

namespace Volo.Docs.Documents.Rendering;

public class ScribanDocumentSectionRenderer : IDocumentSectionRenderer
{
    protected readonly static List<DocsJsonSection> DocsJsonSections =
    [
        new("````json", "````"),
        new("```json", "```")
    ];

    protected const string DocsParam = "//[doc-params]";
    protected const string DocsTemplates = "//[doc-template]";
    protected const string DocsNav = "//[doc-nav]";

    public ILogger<ScribanDocumentSectionRenderer> Logger { get; set; }

    public ScribanDocumentSectionRenderer()
    {
        Logger = NullLogger<ScribanDocumentSectionRenderer>.Instance;
    }

    public async Task<string> RenderAsync(
        string document,
        DocumentRenderParameters parameters = null,
        List<DocumentPartialTemplateContent> partialTemplates = null)
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

        return RemoveOptionsJson(result, DocsParam, DocsNav);
    }

    public Task<Dictionary<string, List<string>>> GetAvailableParametersAsync(string document)
    {
        return GetSectionAsync<Dictionary<string, List<string>>>(document, DocsParam);
    }

    protected virtual async Task<T> GetSectionAsync<T>(string document, string sectionName) where T : new()
    {
        try
        {
            if (!HasJsonSection(document) || !document.Contains(sectionName))
            {
                return new T();
            }

            var (jsonBeginningIndex, jsonEndingIndex, insideJsonSection, _) =
                GetJsonBeginEndIndexesAndPureJson(document, sectionName);

            if (jsonBeginningIndex < 0 || jsonEndingIndex <= 0 || string.IsNullOrWhiteSpace(insideJsonSection))
            {
                return new T();
            }

            var pureJson = insideJsonSection.Replace(sectionName, "").Trim();

            if (!DocsJsonSerializerHelper.TryDeserialize<T>(pureJson, out var section))
            {
                throw new UserFriendlyException($"ERROR-20200327: Cannot validate JSON content for `{sectionName}`!");
            }

            return await Task.FromResult(section);
        }
        catch (Exception)
        {
            Logger.LogWarning("Unable to parse parameters of document.");
            return new T();
        }
    }

    protected static string RemoveOptionsJson(string document, params string[] sectionNames)
    {
        foreach (var sectionName in sectionNames)
        {
            var orgDocument = document;

            try
            {
                if (!HasJsonSection(document) || !document.Contains(sectionName))
                {
                    continue;
                }

                var (jsonBeginningIndex, jsonEndingIndex, insideJsonSection, jsonSection) =
                    GetJsonBeginEndIndexesAndPureJson(document, sectionName);

                if (jsonBeginningIndex < 0 || jsonEndingIndex <= 0 || string.IsNullOrWhiteSpace(insideJsonSection))
                {
                    continue;
                }

                document = document.Remove(
                    jsonBeginningIndex - jsonSection.Opener.Length,
                    (jsonEndingIndex + jsonSection.Closer.Length) - (jsonBeginningIndex - jsonSection.Opener.Length)
                );
            }
            catch (Exception)
            {
                document = orgDocument;
            }
        }

        return document;
    }

    protected static bool HasJsonSection(string document)
    {
        return DocsJsonSections.Any(section => document.Contains(section.Opener) && document.Contains(section.Closer));
    }

    protected static (int, int, string, DocsJsonSection) GetJsonBeginEndIndexesAndPureJson(string document, string sectionName)
    {
        foreach (var section in DocsJsonSections)
        {
            var (jsonBeginningIndex, jsonEndingIndex, insideJsonSection) =
                section.GetJsonBeginEndIndexesAndPureJson(document, sectionName);

            if (jsonBeginningIndex >= 0 && jsonEndingIndex > 0 && !string.IsNullOrWhiteSpace(insideJsonSection))
            {
                return (jsonBeginningIndex, jsonEndingIndex, insideJsonSection, section);
            }
        }

        return (-1, -1, "", null);
    }

    protected static string SetPartialTemplates(string document, IReadOnlyCollection<DocumentPartialTemplateContent> templates)
    {
        foreach (var section in DocsJsonSections)
        {
            document = section.SetPartialTemplates(document, templates);
        }

        return document;
    }

    protected class DocsJsonSection
    {
        public string Opener { get; }
        public string Closer { get; }

        public DocsJsonSection(string opener, string closer)
        {
            Opener = opener;
            Closer = closer;
        }

        public (int, int, string) GetJsonBeginEndIndexesAndPureJson(string document, string sectionName)
        {
            var searchedIndex = 0;

            while (searchedIndex < document.Length)
            {
                var jsonBeginningIndex = document.Substring(searchedIndex).IndexOf(Opener, StringComparison.Ordinal);

                if (jsonBeginningIndex < 0)
                {
                    return (-1, -1, "");
                }

                jsonBeginningIndex += Opener.Length + searchedIndex;

                var jsonEndingIndex = document.Substring(jsonBeginningIndex).IndexOf(Closer, StringComparison.Ordinal);
                if (jsonEndingIndex < 0)
                {
                    return (-1, -1, "");
                }

                jsonEndingIndex += jsonBeginningIndex;
                var insideJsonSection = document.Substring(jsonBeginningIndex, jsonEndingIndex - jsonBeginningIndex);

                if (insideJsonSection.IndexOf(sectionName, StringComparison.Ordinal) < 0)
                {
                    searchedIndex = jsonEndingIndex + Closer.Length;
                    continue;
                }

                return (jsonBeginningIndex, jsonEndingIndex, insideJsonSection);
            }

            return (-1, -1, "");
        }

        public async Task<List<DocumentPartialTemplateWithValues>> GetPartialTemplatesInDocumentAsync(string documentContent)
        {
            var templates = new List<DocumentPartialTemplateWithValues>();

            while (documentContent.Contains(Opener))
            {
                var afterJsonOpener = documentContent.Substring(
                    documentContent.IndexOf(Opener, StringComparison.Ordinal) + Opener.Length
                );

                var betweenJsonOpenerAndCloser = afterJsonOpener.Substring(0,
                    afterJsonOpener.IndexOf(Closer, StringComparison.Ordinal)
                );

                documentContent = afterJsonOpener.Substring(
                    afterJsonOpener.IndexOf(Closer, StringComparison.Ordinal) + Closer.Length
                );

                if (!betweenJsonOpenerAndCloser.Contains(DocsTemplates))
                {
                    continue;
                }

                var json = betweenJsonOpenerAndCloser.Substring(
                    betweenJsonOpenerAndCloser.IndexOf(DocsTemplates, StringComparison.Ordinal) +
                    DocsTemplates.Length);

                if (!DocsJsonSerializerHelper.TryDeserialize<DocumentPartialTemplateWithValues>(json,
                        out var template))
                {
                    throw new UserFriendlyException(
                        $"ERROR-20200327: Cannot validate JSON content for `AvailableParameters`!");
                }

                templates.Add(template);
            }

            return await Task.FromResult(templates);
        }

        public string SetPartialTemplates(string document, IReadOnlyCollection<DocumentPartialTemplateContent> templates)
        {
            var newDocument = new StringBuilder();

            while (document.Contains(Opener))
            {
                var beforeJson = document.Substring(0,
                    document.IndexOf(Opener, StringComparison.Ordinal) + Opener.Length
                );

                var afterJsonOpener = document.Substring(
                    document.IndexOf(Opener, StringComparison.Ordinal) + Opener.Length
                );

                var betweenJsonOpenerAndCloser = afterJsonOpener.Substring(0,
                    afterJsonOpener.IndexOf(Closer, StringComparison.Ordinal)
                );

                if (!betweenJsonOpenerAndCloser.Contains(DocsTemplates))
                {
                    document = afterJsonOpener.Substring(
                        afterJsonOpener.IndexOf(Closer, StringComparison.Ordinal) + Closer.Length
                    );

                    newDocument.Append(beforeJson + betweenJsonOpenerAndCloser + Closer);
                    continue;
                }

                var json = betweenJsonOpenerAndCloser.Substring(
                    betweenJsonOpenerAndCloser.IndexOf(DocsTemplates, StringComparison.Ordinal) +
                    DocsTemplates.Length
                );

                if (DocsJsonSerializerHelper.TryDeserialize<DocumentPartialTemplateWithValues>(json,
                        out var documentPartialTemplateWithValuesDto))
                {
                    var template =
                        templates.FirstOrDefault(t => t.Path == documentPartialTemplateWithValuesDto.Path);

                    var beforeTemplate = document.Substring(0,
                        document.IndexOf(Opener, StringComparison.Ordinal)
                    );

                    newDocument.Append(beforeTemplate + template?.Content + Closer);

                    document = afterJsonOpener.Substring(
                        afterJsonOpener.IndexOf(Closer, StringComparison.Ordinal) + Closer.Length
                    );
                }
            }

            newDocument.Append(document);

            return newDocument.ToString();
        }
    }
}