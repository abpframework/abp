using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NuGet.Versioning;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Licensing;
using Volo.Abp.Cli.ProjectBuilding.Analyticses;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectBuilding;

public class TemplateProjectBuilder : IProjectBuilder, ITransientDependency
{
    public ILogger<TemplateProjectBuilder> Logger { get; set; }

    protected ISourceCodeStore SourceCodeStore { get; }
    protected ITemplateInfoProvider TemplateInfoProvider { get; }
    protected ICliAnalyticsCollect CliAnalyticsCollect { get; }
    protected AbpCliOptions Options { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IApiKeyService ApiKeyService { get; }

    private readonly IConfiguration _configuration;

    public TemplateProjectBuilder(ISourceCodeStore sourceCodeStore,
        ITemplateInfoProvider templateInfoProvider,
        ICliAnalyticsCollect cliAnalyticsCollect,
        IOptions<AbpCliOptions> options,
        IJsonSerializer jsonSerializer,
        IApiKeyService apiKeyService,
        IConfiguration configuration)
    {
        SourceCodeStore = sourceCodeStore;
        TemplateInfoProvider = templateInfoProvider;
        CliAnalyticsCollect = cliAnalyticsCollect;
        Options = options.Value;
        JsonSerializer = jsonSerializer;
        ApiKeyService = apiKeyService;
        _configuration = configuration;

        Logger = NullLogger<TemplateProjectBuilder>.Instance;
    }

    public async Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args)
    {
        var templateInfo = await GetTemplateInfoAsync(args);

        NormalizeArgs(args, templateInfo);

        var templateFile = await SourceCodeStore.GetAsync(
            args.TemplateName,
            SourceCodeTypes.Template,
            args.Version,
            args.TemplateSource,
            args.ExtraProperties.ContainsKey(NewCommand.Options.Preview.Long)
        );

        DeveloperApiKeyResult apiKeyResult = null;

#if DEBUG
        try
        {
            var apiKeyResultSection = _configuration.GetSection("apiKeyResult");
            if (apiKeyResultSection.Exists())
            {
                apiKeyResult = apiKeyResultSection.Get<DeveloperApiKeyResult>(); //you can use user secrets
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (apiKeyResult == null)
        {
            apiKeyResult = await ApiKeyService.GetApiKeyOrNullAsync();
        }
#else
            apiKeyResult = await ApiKeyService.GetApiKeyOrNullAsync();
#endif

        if (apiKeyResult != null)
        {
            if (apiKeyResult.ApiKey != null)
            {
                args.ExtraProperties["api-key"] = apiKeyResult.ApiKey;
            }
            else if (templateInfo.Name == AppProTemplate.TemplateName)
            {
                throw new UserFriendlyException(apiKeyResult.ErrorMessage);
            }
        }

        if (apiKeyResult?.LicenseCode != null)
        {
            args.ExtraProperties["license-code"] = apiKeyResult.LicenseCode;
        }

        var context = new ProjectBuildContext(
            templateInfo,
            null,
            null,
            null,
            templateFile,
            args
        );

        if (context.Template is AppTemplateBase appTemplateBase)
        {
            appTemplateBase.HasDbMigrations = SemanticVersion.Parse(templateFile.Version) < new SemanticVersion(4, 3, 99);
        }

        TemplateProjectBuildPipelineBuilder.Build(context).Execute();

        if (!templateInfo.DocumentUrl.IsNullOrEmpty())
        {
            Logger.LogInformation("Check out the documents at " + templateInfo.DocumentUrl);
        }

        // Exclude unwanted or known options.
        var options = args.ExtraProperties
            .Where(x => !x.Key.Equals(CliConsts.Command, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.Key.Equals(NewCommand.Options.Tiered.Long, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.Key.Equals(NewCommand.Options.Preview.Long, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.Key.Equals(NewCommand.Options.DatabaseProvider.Long, StringComparison.InvariantCultureIgnoreCase) &&
                        !x.Key.Equals(NewCommand.Options.DatabaseProvider.Short, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.Key.Equals(NewCommand.Options.OutputFolder.Long, StringComparison.InvariantCultureIgnoreCase) &&
                        !x.Key.Equals(NewCommand.Options.OutputFolder.Short, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.Key.Equals(NewCommand.Options.UiFramework.Long, StringComparison.InvariantCultureIgnoreCase) &&
                        !x.Key.Equals(NewCommand.Options.UiFramework.Short, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.Key.Equals(NewCommand.Options.Mobile.Long, StringComparison.InvariantCultureIgnoreCase) &&
                        !x.Key.Equals(NewCommand.Options.Mobile.Short, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.Key.Equals(NewCommand.Options.Version.Long, StringComparison.InvariantCultureIgnoreCase) &&
                        !x.Key.Equals(NewCommand.Options.Version.Short, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.Key.Equals(NewCommand.Options.TemplateSource.Short, StringComparison.InvariantCultureIgnoreCase) &&
                        !x.Key.Equals(NewCommand.Options.TemplateSource.Long, StringComparison.InvariantCultureIgnoreCase))
            .Select(x => x.Key).ToList();

        await CliAnalyticsCollect.CollectAsync(new CliAnalyticsCollectInputDto
        {
            Tool = Options.ToolName,
            Command = args.ExtraProperties.ContainsKey(CliConsts.Command) ? args.ExtraProperties[CliConsts.Command] : "",
            DatabaseProvider = args.DatabaseProvider.ToProviderName(),
            IsTiered = args.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long),
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

    private async Task<TemplateInfo> GetTemplateInfoAsync(ProjectBuildArgs args)
    {
        if (args.TemplateName.IsNullOrWhiteSpace())
        {
            return await TemplateInfoProvider.GetDefaultAsync();
        }
        else
        {
            return TemplateInfoProvider.Get(args.TemplateName);
        }
    }
}
