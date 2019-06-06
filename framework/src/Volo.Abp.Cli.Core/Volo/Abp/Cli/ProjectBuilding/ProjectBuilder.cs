using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class ProjectBuilder : IProjectBuilder, ITransientDependency
    {
        public ILogger<ProjectBuilder> Logger { get; set; }

        protected ITemplateStore TemplateStore { get; }
        protected ITemplateInfoProvider TemplateInfoProvider { get; }

        public ProjectBuilder(ITemplateStore templateStore, ITemplateInfoProvider templateInfoProvider)
        {
            TemplateStore = templateStore;
            TemplateInfoProvider = templateInfoProvider;

            Logger = NullLogger<ProjectBuilder>.Instance;
        }
        
        public async Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args)
        {
            var templateInfo = GetTemplateInfo(args);

            args.TemplateName = templateInfo.Name;

            if (args.DatabaseProvider == DatabaseProvider.NotSpecified)
            {
                if (templateInfo.DefaultDatabaseProvider != DatabaseProvider.NotSpecified)
                {
                    args.DatabaseProvider = templateInfo.DefaultDatabaseProvider;
                }
            }

            var templateFile = await TemplateStore.GetAsync(
                args.TemplateName,
                args.DatabaseProvider,
                args.SolutionName.FullName
            );

            var context = new ProjectBuildContext(
                templateInfo,
                templateFile,
                args
            );

            ProjectBuildPipelineBuilder.Build(context).Execute(context);

            if (!templateInfo.DocumentUrl.IsNullOrEmpty())
            {
                Logger.LogInformation("Check the documentation of this template: " + templateInfo.DocumentUrl);
            }

            return new ProjectBuildResult(context.Result.ZipContent, args.SolutionName.ProjectName);
        }

        private TemplateInfo GetTemplateInfo(ProjectBuildArgs args)
        {
            if (args.TemplateName.IsNullOrWhiteSpace())
            {
                return TemplateInfoProvider.GetDefault();
            }
            else
            {
                return TemplateInfoProvider.Get(args.TemplateName);
            }
        }
    }
}
