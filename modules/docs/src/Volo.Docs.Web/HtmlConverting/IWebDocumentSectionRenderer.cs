using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents.Rendering;

namespace Volo.Docs.HtmlConverting
{
    public interface IWebDocumentSectionRenderer: IDocumentSectionRenderer
    {
        Task<List<DocumentPartialTemplateWithValues>> GetPartialTemplatesInDocumentAsync(string documentContent);
        
        Task<DocumentNavigationsDto> GetDocumentNavigationsAsync(string documentContent);
    }
}
