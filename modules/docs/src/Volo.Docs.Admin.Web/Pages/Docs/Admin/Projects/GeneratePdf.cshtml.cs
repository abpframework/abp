using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Common.Documents;
using Volo.Docs.Common.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Projects;

public class GeneratePdfModal : DocsAdminPageModel
{
    protected IProjectAppService ProjectAppService { get; }
    protected IProjectAdminAppService ProjectAdminAppService { get; }
    protected IDocumentPdfAdminAppService DocumentPdfAdminAppService { get; }

    public GeneratePdfViewModel ViewModel { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public Guid ProjectId { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string Version { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string Language { get; set; }

    public GeneratePdfModal(
        IProjectAppService projectAppService, 
        IProjectAdminAppService projectAdminAppService, 
        IDocumentPdfAdminAppService documentPdfAdminAppService)
    {
        ProjectAppService = projectAppService;
        ProjectAdminAppService = projectAdminAppService;
        DocumentPdfAdminAppService = documentPdfAdminAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var project = await ProjectAdminAppService.GetAsync(ProjectId);
        var versions = await ProjectAppService.GetVersionsAsync(project.ShortName);
        if(versions.Items.Count == 0)
        {
            versions.Items =
            [
                new VersionInfoDto { Name = ProjectConsts.Latest, DisplayName = ProjectConsts.Latest }
            ];
        }
        var languages = await ProjectAppService.GetLanguageListAsync(project.ShortName, versions.Items.FirstOrDefault()?.Name);
        ViewModel = new GeneratePdfViewModel
        {
            ShortName = project.ShortName,
            Versions = versions.Items.Select(x => new SelectListItem(x.DisplayName, x.Name)).ToList(),
            Languages = languages.Languages.Select(x => new SelectListItem(x.DisplayName, x.Code)).ToList()
        };

        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await DocumentPdfAdminAppService.GeneratePdfAsync(new DocumentPdfGeneratorInput
        {
            ProjectId = ProjectId,
            Version = Version,
            LanguageCode = Language
        });
        
        return NoContent();
    }

    public class GeneratePdfViewModel
    {
        public string ShortName { get; set; }
        public List<SelectListItem>  Versions { get; set; }
        public List<SelectListItem>  Languages { get; set; }
    }
}