using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Documents.Rendering;

public interface IDocumentSectionRenderer : ITransientDependency
{
    Task<string> RenderAsync(string document, DocumentRenderParameters parameters = null, List<DocumentPartialTemplateContent> partialTemplates = null);

    Task<Dictionary<string, List<string>>> GetAvailableParametersAsync(string document);
}