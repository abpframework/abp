using Microsoft.Extensions.Configuration;
using Volo.Utils.SolutionTemplating.Building;

namespace Volo.AbpWebSite.Templates
{
    public class MvcModuleTemplate : TemplateInfo
    {
        public MvcModuleTemplate(IConfigurationRoot configuration) 
            : base(
                "abp-mvc-module",
                new GithubRepositoryInfo("abpframework/abp", configuration["GithubAccessToken"]),
                "/templates/module")
        {

        }
    }
}
