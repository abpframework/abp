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
    public class NugetPackageProjectBuilder : IProjectBuilder, ITransientDependency
    {
        public ILogger<NugetPackageProjectBuilder> Logger { get; set; }
        protected ISourceCodeStore SourceCodeStore { get; }
        protected INugetPackageInfoProvider NugetPackageInfoProvider { get; }
        protected ICliAnalyticsCollect CliAnalyticsCollect { get; }
        protected AbpCliOptions Options { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected IApiKeyService ApiKeyService { get; }

        public NugetPackageProjectBuilder(ISourceCodeStore sourceCodeStore,
            INugetPackageInfoProvider nugetPackageInfoProvider,
            ICliAnalyticsCollect cliAnalyticsCollect,
            IOptions<AbpCliOptions> options,
            IJsonSerializer jsonSerializer,
            IApiKeyService apiKeyService)
        {
            SourceCodeStore = sourceCodeStore;
            NugetPackageInfoProvider = nugetPackageInfoProvider;
            CliAnalyticsCollect = cliAnalyticsCollect;
            Options = options.Value;
            JsonSerializer = jsonSerializer;
            ApiKeyService = apiKeyService;

            Logger = NullLogger<NugetPackageProjectBuilder>.Instance;
        }

        public async Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args)
        {
            var packageInfo = await GetPackageInfoAsync(args);

            var templateFile = await SourceCodeStore.GetAsync(
                args.TemplateName,
                SourceCodeTypes.NugetPackage,
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
                packageInfo,
                null,
                templateFile,
                args
            );

            NugetPackageProjectBuildPipelineBuilder.Build(context).Execute();

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

        private async Task<NugetPackageInfo> GetPackageInfoAsync(ProjectBuildArgs args)
        {
            return await NugetPackageInfoProvider.GetAsync(args.TemplateName);
        }
    }
}
