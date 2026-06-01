using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.Documents.Rendering;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Utils;
using Volo.Extensions;

namespace Volo.Docs.Projects.Pdf;

public class ProjectPdfGenerator : IProjectPdfGenerator, ITransientDependency
{
    protected IDocumentSourceFactory DocumentStoreFactory { get; }
    protected IDocumentToHtmlConverterFactory DocumentToHtmlConverterFactory { get; }
    protected IDocumentRepository DocumentRepository { get; }
    protected IDocumentSectionRenderer DocumentSectionRenderer { get; }
    protected IOptions<DocsProjectPdfGeneratorOptions> Options { get; }
    protected IProjectPdfFileStore ProjectPdfFileStore { get; }
    protected IHtmlToPdfRenderer HtmlToPdfRenderer { get; }
    protected ILogger<ProjectPdfGenerator> Logger { get; set; }
    protected IDocumentSource DocumentSource { get; set; }
    protected DocumentParams DocumentParams { get; set; }
    protected Project Project { get; set; }
    protected List<PdfDocument> AllPdfDocuments { get; } = [];

    public ProjectPdfGenerator(
        IDocumentSourceFactory documentStoreFactory,
        IDocumentRepository documentRepository,
        IOptions<DocsProjectPdfGeneratorOptions> options,
        IDocumentSectionRenderer documentSectionRenderer,
        IProjectPdfFileStore projectPdfFileStore,
        IHtmlToPdfRenderer htmlToPdfRenderer,
        IDocumentToHtmlConverterFactory documentToHtmlConverterFactory,
        ILogger<ProjectPdfGenerator> logger)
    {
        DocumentStoreFactory = documentStoreFactory;
        DocumentRepository = documentRepository;
        Options = options;
        DocumentSectionRenderer = documentSectionRenderer;
        ProjectPdfFileStore = projectPdfFileStore;
        HtmlToPdfRenderer = htmlToPdfRenderer;
        DocumentToHtmlConverterFactory = documentToHtmlConverterFactory;
        Logger = logger;
    }

    public virtual async Task GenerateAsync(Project project, string version, string languageCode)
    {
        Project = project;
        DocumentSource = DocumentStoreFactory.Create(project.DocumentStoreType);
        DocumentParams = await GetDocumentParamsAsync(project, version, languageCode);

        var navigation = await GetNavigationAsync(project, version, languageCode);
        await SetAllPdfDocumentsAsync(navigation.Items, project, version, languageCode);

        var title = Options.Value.CalculatePdfFileTitle?.Invoke(project) ?? project.Name;
        var tempFiles = new List<(PdfDocument pdfDocument, Stream stream)>();

        try
        {
            for (var i = 0; i < AllPdfDocuments.Count; i++)
            {
                var document = AllPdfDocuments[i];
                Logger.LogInformation("Processing chunk {Index}/{Total}", i + 1, AllPdfDocuments.Count);

                var chunkHtml = await BuildHtmlAsync([document]);

                var pdfStream = await HtmlToPdfRenderer.RenderAsync($"{title} - Part {document.Title}", chunkHtml, [document]);

                Logger.LogInformation("Chunk {Index} rendered to PDF", i + 1);

                tempFiles.Add((document, pdfStream));
            }

            Logger.LogInformation("All chunks processed, merging PDF files");

            var mergedPdfStream = await MergePdfFilesAsync(tempFiles);
            await ProjectPdfFileStore.SetAsync(project, version, languageCode, mergedPdfStream);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occurred while generating the PDF for project {ProjectName}", project.Name);
            foreach (var tempStream in tempFiles)
            {
                try
                {
                    await tempStream.stream.DisposeAsync();
                }
                catch
                {
                    // ignore any exceptions during disposal
                }
            }
        }
        finally
        {
            AllPdfDocuments.Clear();
        }
    }

    protected virtual async Task<string> BuildHtmlAsync(List<PdfDocument> pdfDocuments)
    {
        var htmlContent = await ConvertDocumentsToHtmlAsync(pdfDocuments);

        var htmlBuilder = new StringBuilder();
        htmlBuilder.Append(Options.Value.HtmlLayout);
        htmlBuilder.Replace(DocsProjectPdfGeneratorOptions.StylePlaceholder, Options.Value.HtmlStyle);
        htmlBuilder.Replace(DocsProjectPdfGeneratorOptions.ContentPlaceholder, htmlContent);

        return htmlBuilder.ToString();
    }

    protected virtual async Task<string> ConvertDocumentsToHtmlAsync(List<PdfDocument> pdfDocuments)
    {
        var contentBuilder = new StringBuilder();

        foreach (var pdfDocument in pdfDocuments)
        {
            if (pdfDocument.Document != null)
            {
                var renderedDocument = await RenderDocumentAsync(pdfDocument);
                var documentToHtmlConverter = GetDocumentToHtmlConverter(Project, pdfDocument);
                var htmlContent =
                    documentToHtmlConverter.Convert(
                        new PdfDocumentToHtmlConverterContext(renderedDocument, pdfDocument, DocumentParams));
                contentBuilder.AppendLine(htmlContent);
            }

            if (pdfDocument.HasChildren)
            {
                contentBuilder.AppendLine(await ConvertDocumentsToHtmlAsync(pdfDocument.Children));
            }
        }

        return contentBuilder.ToString();
    }

    protected virtual async Task<MemoryStream> MergePdfFilesAsync(
        List<(PdfDocument pdfDocument, Stream stream)> pdfFiles)
    {
        if (pdfFiles.Count == 0)
        {
            throw new ArgumentException("No PDF files to merge", nameof(pdfFiles));
        }

        var zipStream = new MemoryStream();


        using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            for (var index = 0; index < pdfFiles.Count; index++)
            {
                var (doc, pdfFile) = pdfFiles[index];
                var entry = zipArchive.CreateEntry($"{index + 1}_{doc.Title}.pdf", CompressionLevel.Fastest);
                await using var entryStream = entry.Open();
                await pdfFile.CopyToAsync(entryStream);

                try
                {
                    await pdfFile.DisposeAsync();
                }
                catch (Exception e)
                {
                    Logger.LogWarning(e, "Error disposing PDF file stream for document {DocumentTitle}", doc.Title);
                }
            }
        }


        zipStream.Position = 0;
        return zipStream;
    }


    protected virtual IDocumentToHtmlConverter<PdfDocumentToHtmlConverterContext> GetDocumentToHtmlConverter(
        Project project, PdfDocument pdfDocument)
    {
        return DocumentToHtmlConverterFactory.Create<PdfDocumentToHtmlConverterContext>(
            DocsDomainConsts.PdfDocumentToHtmlConverterPrefix + (pdfDocument.Document.Format ?? project.Format));
    }

    protected virtual async Task<string> RenderDocumentAsync(PdfDocument pdfDocument)
    {
        var parameters = new DocumentRenderParameters();
        if (pdfDocument.RenderParameters == null)
        {
            return await DocumentSectionRenderer.RenderAsync(pdfDocument.Document.Content, parameters);
        }

        foreach (var renderParameter in pdfDocument.RenderParameters)
        {
            var documentParam = DocumentParams.Parameters.FirstOrDefault(p => p.Name == renderParameter.Key);
            parameters.Add(renderParameter.Key, renderParameter.Value);
            parameters.Add(renderParameter.Key + "_Value",
                documentParam?.Values[renderParameter.Value] ?? renderParameter.Value);
        }

        return await DocumentSectionRenderer.RenderAsync(pdfDocument.Document.Content, parameters);
    }

    protected virtual async Task SetAllPdfDocumentsAsync(
        List<NavigationNode> navigations,
        Project project,
        string version,
        string languageCode,
        PdfDocument parentPdfDocument = null)
    {
        var groupedCombinationsDocuments = new Dictionary<string, List<PdfDocument>>();

        foreach (var navigation in navigations)
        {
            if (navigation.IgnoreOnDownload)
            {
                continue;
            }

            try
            {
                var pdfDocument = new PdfDocument
                {
                    Title = navigation.Text,
                    IgnoreOnOutline = navigation.Path == Options.Value.IndexPagePath,
                    Id = UrlHelper.IsExternalLink(navigation.Path) ? navigation.Path : null
                };

                if (!navigation.Path.IsNullOrWhiteSpace() && !UrlHelper.IsExternalLink(navigation.Path) &&
                    !navigation.HasChildItems)
                {
                    await HandleLeafDocumentAsync(
                        navigation,
                        project,
                        version,
                        languageCode,
                        parentPdfDocument,
                        groupedCombinationsDocuments,
                        pdfDocument
                    );
                }

                if (!navigation.IsInSeries)
                {
                    if (parentPdfDocument == null)
                    {
                        AllPdfDocuments.AddIfNotContains(pdfDocument);
                    }
                    else
                    {
                        parentPdfDocument.Children.AddIfNotContains(pdfDocument);
                    }
                }

                if (navigation.HasChildItems)
                {
                    AddParameterCombinationsDocuments(parentPdfDocument, groupedCombinationsDocuments);
                    await SetAllPdfDocumentsAsync(navigation.Items, project, version, languageCode, pdfDocument);
                }

                if (!navigation.IsInSeries || navigation == navigations.Last())
                {
                    AddParameterCombinationsDocuments(parentPdfDocument, groupedCombinationsDocuments);
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning(e,
                    $"Cannot get document for the path '{navigation.Path}' in the project {project.Name}.");
            }
        }
    }

    private async Task HandleLeafDocumentAsync(
        NavigationNode navigation,
        Project project,
        string version,
        string languageCode,
        PdfDocument parentPdfDocument,
        Dictionary<string, List<PdfDocument>> groupedCombinationsDocuments,
        PdfDocument leafDocument)
    {
        var document = await GetDocumentAsync(project, navigation.Path, version, languageCode);
        var parameters = await DocumentSectionRenderer.GetAvailableParametersAsync(document.Content);
        var parameterCombinations = GenerateAllParameterCombinations(parameters.Keys.ToList(), parameters);
        var firstCombination = parameterCombinations.FirstOrDefault();

        leafDocument.Document = document;
        leafDocument.RenderParameters = firstCombination;
        leafDocument.Id = GetDocumentId(document.Name, document.Format ?? project.Format, firstCombination, true);
        leafDocument.Title = GetDocumentTitle(navigation.Text, firstCombination, DocumentParams);

        if (parameterCombinations.Count <= 1)
        {
            AddParameterCombinationsDocuments(parentPdfDocument, groupedCombinationsDocuments);
            return;
        }

        for (var i = 0; i < parameterCombinations.Count; i++)
        {
            var combination = parameterCombinations[i];
            var key = string.Join("-", combination.Select(x => $"{x.Key}_{x.Value}"));

            if (!groupedCombinationsDocuments.ContainsKey(key))
            {
                groupedCombinationsDocuments[key] = [];
            }

            var combinationDocument = i == 0
                ? leafDocument
                : new PdfDocument
                {
                    Document = document,
                    Id = GetDocumentId(document.Name, document.Format ?? project.Format, combination, false),
                    Title = GetDocumentTitle(navigation.Text, combination, DocumentParams),
                    RenderParameters = combination
                };

            groupedCombinationsDocuments[key].Add(combinationDocument);
        }
    }

    private void AddParameterCombinationsDocuments(PdfDocument parentPdfDocument,
        Dictionary<string, List<PdfDocument>> groupedCombinationsDocuments)
    {
        foreach (var combinations in groupedCombinationsDocuments)
        {
            if (parentPdfDocument == null)
            {
                AllPdfDocuments.AddIfNotContains(combinations.Value);
            }
            else
            {
                parentPdfDocument.Children.AddIfNotContains(combinations.Value);
            }
        }

        groupedCombinationsDocuments.Clear();
    }

    private async Task<NavigationNode> GetNavigationAsync(
        Project project,
        string version,
        string languageCode)
    {
        var navigationDocument = await GetDocumentAsync(project, project.NavigationDocumentName, version, languageCode);

        if (!DocsJsonSerializerHelper.TryDeserialize<NavigationNode>(navigationDocument.Content, out var navigation))
        {
            throw new UserFriendlyException(
                $"Cannot validate navigation file '{project.NavigationDocumentName}' for the project {project.Name}.");
        }

        if (!Options.Value.IndexPagePath.IsNullOrWhiteSpace())
        {
            navigation.Items.Insert(0, new NavigationNode { Path = Options.Value.IndexPagePath });
        }

        return navigation;
    }

    private async Task<DocumentParams> GetDocumentParamsAsync(
        Project project,
        string version,
        string languageCode)
    {
        if (project.ParametersDocumentName.IsNullOrWhiteSpace())
        {
            return new DocumentParams();
        }

        try
        {
            var documentParamsDocument =
                await GetDocumentAsync(project, project.ParametersDocumentName, version, languageCode);

            if (!DocsJsonSerializerHelper.TryDeserialize<DocumentParams>(documentParamsDocument.Content,
                    out var documentParams))
            {
                throw new UserFriendlyException(
                    $"Cannot validate document params file '{project.ParametersDocumentName}' for the project {project.Name}.");
            }

            return documentParams;
        }
        catch (Exception e)
        {
            Logger.LogError(e, $"Cannot get document params for the project {project.Name}.", e);
            return new DocumentParams();
        }
    }

    private async Task<Document> GetDocumentAsync(
        Project project,
        string documentName,
        string version,
        string languageCode)
    {
        version = string.IsNullOrWhiteSpace(version) ? project.LatestVersionBranchName : version;

        Document document = null;
        Exception firstException = null;

        var possibleNames = GetPossibleNames(documentName, project.Format);
        document = await DocumentRepository.FindAsync(project.Id, possibleNames, languageCode, version);
        if (document != null)
        {
            return document;
        }

        foreach (var name in possibleNames)
        {
            try
            {
                document = await DocumentSource.GetDocumentAsync(project, name, languageCode, version);
                await DocumentRepository.InsertAsync(document, true);
                break;
            }
            catch (Exception ex)
            {
                firstException ??= ex;
            }
        }

        if (document == null)
        {
            throw firstException!;
        }

        return document;
    }

    private static List<DocumentRenderParameters> GenerateAllParameterCombinations(List<string> parameterKeys,
        Dictionary<string, List<string>> parameters)
    {
        var parameterCombinations = new List<DocumentRenderParameters>();

        if (parameters.Count <= 0)
        {
            return parameterCombinations;
        }

        GenerateCombinations(0, new DocumentRenderParameters());
        return parameterCombinations;

        void GenerateCombinations(int keyIndex, DocumentRenderParameters currentCombination)
        {
            if (keyIndex == parameterKeys.Count)
            {
                parameterCombinations.Add(new DocumentRenderParameters(currentCombination));
                return;
            }

            var currentKey = parameterKeys[keyIndex];
            foreach (var value in parameters[currentKey])
            {
                currentCombination[currentKey] = value;
                GenerateCombinations(keyIndex + 1, currentCombination);
            }
        }
    }

    private static string GetDocumentId(string documentName, string format, DocumentRenderParameters parameters,
        bool isFirstCombinationDocument)
    {
        var id = documentName.Replace("." + format, string.Empty).Replace("/", "-").Replace(" ", "-").ToLower();
        if (parameters != null && !isFirstCombinationDocument)
        {
            id = $"{id}{parameters.Select(x => $"{x.Key}_{x.Value}").JoinAsString("-")}";
        }

        return id;
    }

    private static string GetDocumentTitle(string title, DocumentRenderParameters parameters,
        DocumentParams documentParams)
    {
        if (parameters == null || parameters.Count <= 0)
        {
            return title;
        }

        var paramValues = parameters.Select(x =>
        {
            var documentParam = documentParams.Parameters.FirstOrDefault(p => p.Name == x.Key);
            return $"{documentParam?.DisplayName ?? x.Key} : {documentParam?.Values[x.Value] ?? x.Value}";
        });

        return title.Trim() + $" ({string.Join(", ", paramValues)})";
    }

    private static List<string> GetPossibleNames(string originalDocumentName, string format)
    {
        var extension = Path.GetExtension(originalDocumentName);
        if (extension.IsNullOrWhiteSpace())
        {
            extension = "." + format;
        }

        if (!extension.Equals("." + format, StringComparison.OrdinalIgnoreCase))
        {
            return [originalDocumentName];
        }

        var lowerCaseIndex = "index." + format;
        var titleCaseIndex = "Index." + format;
        var indexLength = lowerCaseIndex.Length;

        var possibleNames = new List<string> { originalDocumentName };
        if (originalDocumentName.EndsWith("/" + lowerCaseIndex, StringComparison.OrdinalIgnoreCase) ||
            originalDocumentName.Equals(lowerCaseIndex, StringComparison.OrdinalIgnoreCase))
        {
            var indexPart = originalDocumentName.Right(indexLength);

            var documentNameWithoutIndex =
                originalDocumentName.Left(originalDocumentName.Length - lowerCaseIndex.Length);

            if (indexPart != lowerCaseIndex)
            {
                possibleNames.Add(documentNameWithoutIndex + lowerCaseIndex);
            }

            if (indexPart != titleCaseIndex)
            {
                possibleNames.Add(documentNameWithoutIndex + titleCaseIndex);
            }
        }
        else
        {
            var documentNameWithoutExtension =
                RemoveFileExtensionFromPath(originalDocumentName, format).EnsureEndsWith('/');
            possibleNames.Add(documentNameWithoutExtension + lowerCaseIndex);
            possibleNames.Add(documentNameWithoutExtension + titleCaseIndex);
        }

        return possibleNames;
    }

    private static string RemoveFileExtensionFromPath(string path, string format)
    {
        if (path == null)
        {
            return null;
        }

        return path.EndsWith("." + format)
            ? path.Left(path.Length - format.Length - 1)
            : path;
    }
}