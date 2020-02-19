using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Docs.Pages.Documents.Shared.DocumentNotFoundComponent
{
    public class DocumentNotFoundViewComponent : AbpViewComponent
    {
        private readonly DocsUiOptions _options;

        public DocumentNotFoundViewComponent(IOptions<DocsUiOptions> options)
        {
            _options = options.Value;
        }
        public IViewComponentResult Invoke(DocumentNotFoundPageModel model, string defaultErrorMessageKey)
        {
            model.DocumentsUrlPrefix = _options.RoutePrefix;

            return View("~/Pages/Documents/Shared/DocumentNotFoundComponent/Default.cshtml", model);
        }
    }
}
