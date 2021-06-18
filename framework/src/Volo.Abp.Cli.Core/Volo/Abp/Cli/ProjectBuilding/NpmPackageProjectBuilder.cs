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
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class NpmPackageProjectBuilder : IProjectBuilder, ITransientDependency
    {
        public ILogger<NpmPackageProjectBuilder> Logger { get; set; }
        protected ISourceCodeStore SourceCodeStore { get; }
        protected INpmPackageInfoProvider NpmPackageInfoProvider { get; }
        protected ICliAnalyticsCollect CliAnalyticsCollect { get; }
        protected AbpCliOptions Options { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected IApiKeyService ApiKeyService { get; }

        public NpmPackageProjectBuilder(ISourceCodeStore sourceCodeStore,
            INpmPackageInfoProvider npmPackageInfoProvider,
            ICliAnalyticsCollect cliAnalyticsCollect,
            IOptions<AbpCliOptions> options,
            IJsonSerializer jsonSerializer,
            IApiKeyService apiKeyService)
        {
            SourceCodeStore = sourceCodeStore;
            NpmPackageInfoProvider = npmPackageInfoProvider;
            CliAnalyticsCollect = cliAnalyticsCollect;
            Options = options.Value;
            JsonSerializer = jsonSerializer;
            ApiKeyService = apiKeyService;

            Logger = NullLogger<NpmPackageProjectBuilder>.Instance;
        }

        public async Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args)
        {
            var packageInfo = await GetPackageInfoAsync(args);

            var templateFile = await SourceCodeStore.GetAsync(
                args.TemplateName,
                SourceCodeTypes.NpmPackage,
                args.Version,
                null,
                args.ExtraProperties.ContainsKey(GetSourceCommand.Options.Preview.Long)
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
                null,
                null,
                null,
                packageInfo,
                templateFile,
                args
            );

            NpmPackageProjectBuildPipelineBuilder.Build(context).Execute();

            // Exclude unwanted or known options.
            var options = args.ExtraProperties
                .Where(x => !x.Key.Equals(CliConsts.Command, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.OutputFolder.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.OutputFolder.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.Version.Long, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.Version.Short, StringComparison.InvariantCultureIgnoreCase))
                .Where(x => !x.Key.Equals(NewCommand.Options.TemplateSource.Short, StringComparison.InvariantCultureIgnoreCase) &&
                            !x.Key.Equals(NewCommand.Options.TemplateSource.Long, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Key).ToList();

            await CliAnalyticsCollect.CollectAsync(new CliAnalyticsCollectInputDto
            {
                Tool = Options.ToolName,
                Command = args.ExtraProperties.ContainsKey(CliConsts.Command) ? args.ExtraProperties[CliConsts.Command] : "",
                DatabaseProvider = null,
                IsTiered = null,
                UiFramework = null,
                Options = JsonSerializer.Serialize(options),
                ProjectName = null,
                TemplateName = args.TemplateName,
                TemplateVersion = templateFile.Version
            });

            return new ProjectBuildResult(context.Result.ZipContent, args.TemplateName);
        }

        private async Task<NpmPackageInfo> GetPackageInfoAsync(ProjectBuildArgs args)
        {
            return await NpmPackageInfoProvider.GetAsync(args.TemplateName);
        }
    }
}
