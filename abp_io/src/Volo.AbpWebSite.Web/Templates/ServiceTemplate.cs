using Microsoft.Extensions.Configuration;
using Volo.Utils.SolutionTemplating.Building;

namespace Volo.AbpWebSite.Templates
{
    public class ServiceTemplate : TemplateInfo
    {
        public ServiceTemplate(IConfigurationRoot configuration)
            : base(
                "abp-service",
                new GithubRepositoryInfo("abpframework/abp", configuration["GithubAccessToken"]),
                "/templates/service")
        {

        }
    }
}