using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scriban;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp;
using Volo.Abp.ObjectMapping;
using Volo.Docs.Documents.Rendering;
using Volo.Extensions;

namespace Volo.Docs.HtmlConverting
{
    public class ScribanWebDocumentSectionRenderer : ScribanDocumentSectionRenderer, IWebDocumentSectionRenderer
    {
        private IObjectMapper ObjectMapper { get; set; }

        public Task<DocumentNavigationsDto> GetDocumentNavigationsAsync(string documentContent)
        {
            return GetSectionAsync<DocumentNavigationsDto>(documentContent, DocsNav);
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