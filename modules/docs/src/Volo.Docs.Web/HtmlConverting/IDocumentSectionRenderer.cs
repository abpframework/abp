using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.HtmlConverting
{
    public interface IDocumentSectionRenderer: ITransientDependency
    {
        Task<string> RenderAsync(string doucment, DocumentRenderParameters parameters);

        Task<Dictionary<string, List<string>>> GetAvailableParametersAsync(string document);
    }
}
