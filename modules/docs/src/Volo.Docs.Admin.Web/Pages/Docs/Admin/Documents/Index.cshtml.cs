using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Documents.Filter;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Documents;

[Authorize(DocsAdminPermissions.Projects.Default)]
public class IndexModel : DocsAdminPageModel
{
    private readonly IDocumentAdminAppService _documentAdminAppService;
    public FilterItems FilterItems { get; set; }

    public IndexModel(IDocumentAdminAppService documentAdminAppService)
    {
        _documentAdminAppService = documentAdminAppService;
    }
    

    public string ToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    public virtual async Task<IActionResult> OnGet()
    {
        FilterItems = await _documentAdminAppService.GetFilterItemsAsync();
        return Page();
    }
}
