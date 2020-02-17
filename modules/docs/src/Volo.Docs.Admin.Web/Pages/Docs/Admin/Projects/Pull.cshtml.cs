using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Documents;

namespace Volo.Docs.Admin.Pages.Docs.Admin.Projects
{
    public class PullModel : DocsAdminPageModel
    {
        [BindProperty]
        public PullDocumentViewModel PullDocument { get; set; }

        private readonly IProjectAdminAppService _projectAppService;
        private readonly IDocumentAdminAppService _documentAppService;

        public PullModel(IProjectAdminAppService projectAppService, 
            IDocumentAdminAppService documentAppService)
        {
            _projectAppService = projectAppService;
            _documentAppService = documentAppService;
        }

        public async Task<ActionResult> OnGetAsync(Guid id)
        {
            var project = await _projectAppService.GetAsync(id);

            PullDocument = new PullDocumentViewModel()
            {
                ProjectId = project.Id,
                All = false
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (PullDocument.All)
            {
                await _documentAppService.PullAllAsync(
                    ObjectMapper.Map<PullDocumentViewModel, PullAllDocumentInput>(PullDocument));
            }
            else
            {
                await _documentAppService.PullAsync(
                    ObjectMapper.Map<PullDocumentViewModel, PullDocumentInput>(PullDocument));
            }

            return NoContent();
        }

        public class PullDocumentViewModel
        {
            [HiddenInput]
            public Guid ProjectId { get; set; }

            public bool All { get; set; }

            [Required]
            [StringLength(DocumentConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            [StringLength(DocumentConsts.MaxLanguageCodeNameLength)]
            public string LanguageCode { get; set; }

            [Required]
            [StringLength(DocumentConsts.MaxVersionNameLength)]
            public string Version { get; set; }
        }
    }
}