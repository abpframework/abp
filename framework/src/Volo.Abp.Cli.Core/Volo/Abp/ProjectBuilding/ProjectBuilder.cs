using System;
using System.Threading.Tasks;
using Volo.Abp.Cli;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ProjectBuilding.Building;

namespace Volo.Abp.ProjectBuilding
{
    public class ProjectBuilder : IProjectBuilder, ITransientDependency
    {
        protected ITemplateStore TemplateStore { get; }
        protected ITemplateInfoProvider TemplateInfoProvider { get; }

        public ProjectBuilder(ITemplateStore templateStore, ITemplateInfoProvider templateInfoProvider)
        {
            TemplateStore = templateStore;
            TemplateInfoProvider = templateInfoProvider;
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

            var version = CliService.Version;

            var templateFile = await TemplateStore.GetAsync(args.TemplateName, version);

            var context = new ProjectBuildContext(
                templateInfo,
                templateFile,
                args,
                version
            );

            ProjectBuildPipelineBuilder.Build(context).Execute(context);

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
