using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Docs.Documents.Rendering;

namespace Volo.Docs.HtmlConverting
{
    public class ScribanWebDocumentSectionRenderer : ScribanDocumentSectionRenderer, IWebDocumentSectionRenderer
    {
        public Task<DocumentNavigationsDto> GetDocumentNavigationsAsync(string documentContent)
        {
            return GetSectionAsync<DocumentNavigationsDto>(documentContent, DocsNav);
        }

        public Task<DocumentSeoDto> GetDocumentSeoAsync(string documentContent)
        {
            return GetSectionAsync<DocumentSeoDto>(documentContent, DocsSeo);
        }

        public async Task<List<DocumentPartialTemplateWithValues>> GetPartialTemplatesInDocumentAsync(
            string documentContent)
        {
            var templates = new List<DocumentPartialTemplateWithValues>();

            foreach (var section in DocsJsonSections)
            {
                templates.AddRange(await section.GetPartialTemplatesInDocumentAsync(documentContent));
            }

            return templates;
        }
    }
}
