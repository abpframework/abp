using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Cli.ProjectBuilding.Analyticses;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class ProjectBuilder : IProjectBuilder, ITransientDependency
    {
        public ILogger<ProjectBuilder> Logger { get; set; }

        protected ITemplateStore TemplateStore { get; }
        protected ITemplateInfoProvider TemplateInfoProvider { get; }
        protected ICliAnalyticsCollect CliAnalyticsCollect { get; }
        protected CliOptions Options { get; }
        protected IJsonSerializer JsonSerializer { get; }

        public ProjectBuilder(ITemplateStore templateStore, 
            ITemplateInfoProvider templateInfoProvider,
            ICliAnalyticsCollect cliAnalyticsCollect, 
            IOptions<CliOptions> options,
            IJsonSerializer jsonSerializer)
        {
            TemplateStore = templateStore;
            TemplateInfoProvider = templateInfoProvider;
            CliAnalyticsCollect = cliAnalyticsCollect;
            Options = options.Value;
            JsonSerializer = jsonSerializer;

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

            // Exclude unwanted or known options.
            var options = args.ExtraProperties
                .Where(x => x.Key != CliConsts.Command)
                .Where(x => x.Key != "tiered")
                .Where(x => x.Key != "database-provider" && x.Key != "d")
                .Where(x => x.Key != "output-folder" && x.Key != "o")
                .Select(x => x.Key).ToList();

            await CliAnalyticsCollect.CollectAsync(new CliAnalyticsCollectInputDto
            {
                Tool = Options.ToolName,
                Command = args.ExtraProperties.ContainsKey(CliConsts.Command) ? args.ExtraProperties[CliConsts.Command] : "",
                DatabaseProvider = args.DatabaseProvider.ToProviderName(),
                IsTiered = args.ExtraProperties.ContainsKey("tiered"),
                Options = JsonSerializer.Serialize(options), //TODO: JSON or comma separated string?
                ProjectName = args.SolutionName.FullName,
                TemplateName = args.TemplateName,
                TemplateVersion = templateFile.Version
            });

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
