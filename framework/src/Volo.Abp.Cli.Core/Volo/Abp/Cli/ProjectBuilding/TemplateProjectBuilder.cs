using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Licensing;
using Volo.Abp.Cli.ProjectBuilding.Analyticses;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class TemplateProjectBuilder : IProjectBuilder, ITransientDependency
    {
        public ILogger<TemplateProjectBuilder> Logger { get; set; }

        protected ISourceCodeStore SourceCodeStore { get; }
        protected ITemplateInfoProvider TemplateInfoProvider { get; }
        protected ICliAnalyticsCollect CliAnalyticsCollect { get; }
        protected AbpCliOptions Options { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected IApiKeyService ApiKeyService { get; }

        public TemplateProjectBuilder(ISourceCodeStore sourceCodeStore, 
            ITemplateInfoProvider templateInfoProvider,
            ICliAnalyticsCollect cliAnalyticsCollect, 
            IOptions<AbpCliOptions> options,
            IJsonSerializer jsonSerializer, 
            IApiKeyService apiKeyService)
        {
            SourceCodeStore = sourceCodeStore;
            TemplateInfoProvider = templateInfoProvider;
            CliAnalyticsCollect = cliAnalyticsCollect;
            Options = options.Value;
            JsonSerializer = jsonSerializer;
            ApiKeyService = apiKeyService;

            Logger = NullLogger<TemplateProjectBuilder>.Instance;
        }
        
        public async Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args)
        {
            var templateInfo = GetTemplateInfo(args);

            NormalizeArgs(args, templateInfo);

            var templateFile = await SourceCodeStore.GetAsync(
                args.TemplateName,
                SourceCodeTypes.Template,
                args.Version
            );
             
            var apiKeyResult = await ApiKeyService.GetApiKeyOrNullAsync();
            if (apiKeyResult?.ApiKey != null)
            {
                args.ExtraProperties["api-key"] = apiKeyResult.ApiKey;
            }

            if (apiKeyResult?.LicenseCode != null)
            {
                args.ExtraProperties["license-code"] = apiKeyResult.LicenseCode;
            }

            var context = new ProjectBuildContext(
                templateInfo,
                null,
                templateFile,
                args
            );

            TemplateProjectBuildPipelineBuilder.Build(context).Execute();

            if (!templateInfo.DocumentUrl.IsNullOrEmpty())
            {
                Logger.LogInformation("Check out the documents at " + templateInfo.DocumentUrl);
            }

            // Exclude unwanted or known options.
            var options = args.ExtraProperties
                .Where(x => !x.Key.Equals(CliConsts.Command, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals("tiered", StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.DatabaseProvider.Long, StringComparison.InvariantCultureIgnoreCase) && 
                            !x.Key.Equals(NewCommand.Options.DatabaseProvider.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.OutputFolder.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.OutputFolder.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.UiFramework.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.UiFramework.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.Version.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.Version.Short, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Key).ToList();

            await CliAnalyticsCollect.CollectAsync(new CliAnalyticsCollectInputDto
            {
                Tool = Options.ToolName,
                Command = args.ExtraProperties.ContainsKey(CliConsts.Command) ? args.ExtraProperties[CliConsts.Command] : "",
                DatabaseProvider = args.DatabaseProvider.ToProviderName(),
                IsTiered = args.ExtraProperties.ContainsKey("tiered"),
                UiFramework = args.UiFramework.ToFrameworkName(),
                Options = JsonSerializer.Serialize(options),
                ProjectName = args.SolutionName.FullName,
                TemplateName = args.TemplateName,
                TemplateVersion = templateFile.Version
            });

            return new ProjectBuildResult(context.Result.ZipContent, args.SolutionName.ProjectName);
        }

        private static void NormalizeArgs(ProjectBuildArgs args, TemplateInfo templateInfo)
        {
            if (args.TemplateName.IsNullOrEmpty())
            {
                args.TemplateName = templateInfo.Name;
            }

            if (args.DatabaseProvider == DatabaseProvider.NotSpecified)
            {
                if (templateInfo.DefaultDatabaseProvider != DatabaseProvider.NotSpecified)
                {
                    args.DatabaseProvider = templateInfo.DefaultDatabaseProvider;
                }
            }

            if (args.UiFramework == UiFramework.NotSpecified)
            {
                if (templateInfo.DefaultUiFramework != UiFramework.NotSpecified)
                {
                    args.UiFramework = templateInfo.DefaultUiFramework;
                }
            }
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
