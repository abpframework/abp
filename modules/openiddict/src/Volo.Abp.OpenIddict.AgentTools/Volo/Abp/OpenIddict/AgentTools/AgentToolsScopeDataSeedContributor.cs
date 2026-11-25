using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// Creates the MCP scope (<see cref="AbpAgentToolsOpenIddictOptions.ScopeName"/>) with English and Turkish display names (shown
/// on the consent page) and <see cref="AbpAgentToolsOpenIddictOptions.GetScopeResources"/> as resources. An existing scope is
/// left as it is, so edits made by an administrator survive.
/// </summary>
public class AgentToolsScopeDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public const string EnglishDisplayName = "Use this application's tools on your behalf, with your permissions (AI agents over MCP)";

    public const string TurkishDisplayName = "Bu uygulamanın araçlarını sizin adınıza ve sizin yetkilerinizle kullanma (MCP üzerinden yapay zekâ ajanları)";

    public ILogger<AgentToolsScopeDataSeedContributor> Logger { get; set; } = NullLogger<AgentToolsScopeDataSeedContributor>.Instance;

    protected IOpenIddictScopeManager ScopeManager { get; }

    protected AbpAgentToolsOpenIddictOptions Options { get; }

    public AgentToolsScopeDataSeedContributor(IOpenIddictScopeManager scopeManager, IOptions<AbpAgentToolsOpenIddictOptions> options)
    {
        ScopeManager = scopeManager;
        Options = options.Value;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        // OpenIddict scopes are host data; the seeder also runs once per tenant.
        if (context.TenantId != null || await ScopeManager.FindByNameAsync(Options.ScopeName) != null)
        {
            return;
        }

        var resources = Options.GetScopeResources();
        if (resources.Count == 0)
        {
            Logger.LogWarning(
                "Agent tools: the OpenIddict scope {Scope} was not created because it would have no resource (token audience). Set " +
                "App:SelfUrl, AbpAgentToolsOpenIddictOptions.McpResource or AbpAgentToolsOpenIddictOptions.ScopeResources and seed again.",
                Options.ScopeName);
            return;
        }

        var descriptor = new OpenIddictScopeDescriptor
        {
            Name = Options.ScopeName,
            DisplayName = EnglishDisplayName,
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("en")] = EnglishDisplayName,
                [CultureInfo.GetCultureInfo("tr")] = TurkishDisplayName
            }
        };
        descriptor.Resources.UnionWith(resources);

        await ScopeManager.CreateAsync(descriptor);
        Logger.LogInformation("Agent tools: created the OpenIddict scope {Scope} with the resources [{Resources}].",
            Options.ScopeName, string.Join(", ", resources));
    }
}
