using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Users;
using Volo.Docs;
using Volo.Docs.Projects;

namespace VoloDocs.Pages
{
    public class IndexModel : PageModel
    {
        public IReadOnlyList<ProjectDto> Projects { get; set; }
        public string ConnectionString { get; set; }
        public string CreateProjectLink { get; set; }



        private readonly IProjectAppService _projectAppService;
        private readonly DbConnectionOptions _dbConnectionOptions;
        private readonly  ICurrentUser _currentUser;

        public IndexModel(IProjectAppService projectAppService,
            IOptionsSnapshot<DbConnectionOptions> dbConnectionOptions, ICurrentUser currentUser)
        {
            _projectAppService = projectAppService;
            _currentUser = currentUser;
            _dbConnectionOptions = dbConnectionOptions.Value;
        }

        public async Task<IActionResult> OnGet()
        {
            ConnectionString = _dbConnectionOptions.ConnectionStrings.Default;
            CreateProjectLink = _currentUser.Id.HasValue
                ? "/Docs/Admin/Projects"
                : "/Account/Login?returnUrl=/Docs/Admin/Projects";

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                return RedirectToPage("./Configure");
            }

            Projects = (await _projectAppService.GetListAsync()).Items;
            if (Projects.Count == 1)
            {
                return RedirectToPage("./Project/Index", new
                {
                    projectName = Projects[0].ShortName,
                    version = DocsAppConsts.Latest,
                    documentName = Projects[0].DefaultDocumentName
                });
            }

          

            
            //if (!Projects.Any())
            //{
            //    if (_currentUser.Id.HasValue)
            //    {
            //        return Redirect("./Docs/Admin/Projects");
            //    }

            //    return Redirect("./account/login?returnUrl=/Docs/Admin/Projects");
            //}

           

            return Page();
        }
    }
}