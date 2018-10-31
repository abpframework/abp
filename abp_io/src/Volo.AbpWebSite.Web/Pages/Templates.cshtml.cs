using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Configuration;
using Volo.AbpWebSite.Templates;
using Volo.Utils.SolutionTemplating;
using Volo.Utils.SolutionTemplating.Building;

namespace Volo.AbpWebSite.Pages
{
    public class TemplatesModel : AbpPageModel
    {
        private readonly SolutionBuilder _solutionBuilder;
        private readonly IConfigurationAccessor _configurationAccessor;

        public TemplatesModel(SolutionBuilder solutionBuilder, IConfigurationAccessor configurationAccessor)
        {
            _solutionBuilder = solutionBuilder;
            _configurationAccessor = configurationAccessor;
        }

        [BindProperty]
        public string CompanyAndProjectName { get; set; }

        [BindProperty]
        public string ProjectType { get; set; }

        [BindProperty]
        public string Version { get; set; } = StandardVersions.LatestStable;

        [BindProperty]
        public DatabaseProvider DatabaseProvider { get; set; } = DatabaseProvider.EntityFrameworkCore;

        [BindProperty]
        [Display(Name = "Replace local references by nuget packages.")]
        public bool ReplaceLocalReferencesToNuget { get; set; } = true;

        public void OnGet()
        {

        }

        public async Task<ActionResult> OnPostAsync()
        {
            var template = CreateTemplateInfo();

            var result = await _solutionBuilder.BuildAsync(
                template,
                CompanyAndProjectName,
                DatabaseProvider,
                Version,
                ReplaceLocalReferencesToNuget
            );

            return File(result.ZipContent, "application/zip", result.ProjectName + ".zip");
        }

        private TemplateInfo CreateTemplateInfo()
        {
            switch (ProjectType)
            {
                case "MvcModule":
                    DatabaseProvider = DatabaseProvider.Irrelevant;
                    return new MvcModuleTemplate(_configurationAccessor.Configuration);
                case "Service":
                    DatabaseProvider = DatabaseProvider.Irrelevant;
                    return new ServiceTemplate(_configurationAccessor.Configuration);
                case "MvcApp":
                default:
                    return new MvcApplicationTemplate(_configurationAccessor.Configuration);
            }
        }
    }
}