using System;
using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectBuilding
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
