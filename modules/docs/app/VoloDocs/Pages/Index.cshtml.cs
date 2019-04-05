using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Docs;
using Volo.Docs.Projects;

namespace VoloDocs.Pages
{
    public class IndexModel : PageModel
    {
        public IReadOnlyList<ProjectDto> Projects { get; set; }
        private readonly IProjectAppService _projectAppService;

        

        public IndexModel(IProjectAppService projectAppService)
        {
            _projectAppService = projectAppService;
        }

        public async Task<IActionResult> OnGet()
        {
            //var listResult = await _projectAppService.GetListAsync();

            //if (listResult.Items.Any())
            //{
            //    return RedirectToPage("./Project/Index", new
            //    {
            //        projectName = listResult.Items[0].ShortName,
            //        version = DocsAppConsts.Latest,
            //        documentName = listResult.Items[0].DefaultDocumentName
            //    });
            //}
            //else
            //{

            //}


            return Page();
        }
    }
}