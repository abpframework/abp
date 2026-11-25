using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Validation;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Threading;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// OpenIddict server and validation defaults the MCP endpoint relies on (see <see cref="AbpAgentToolsOpenIddictOptions"/>), the
/// <c>AgentTools</c> scope seed, the optional MCP client seed from configuration, the MCP resource permission sync
/// (<see cref="AgentToolsOpenIddictApplicationStore"/>, <see cref="AgentToolsMcpResourcePermissionDataSeedContributor"/>) and a
/// startup check.
/// Applied as options configuration after ABP's and the app's PreConfigure&lt;OpenIddictBuilder&gt; actions, so it works whatever
/// order the app configures OpenIddict in.
/// </summary>
[DependsOn(typeof(AbpOpenIddictAspNetCoreModule))]
public class AbpOpenIddictAgentToolsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<AbpAgentToolsOpenIddictOptions>().Configure<IConfiguration>((options, configuration) =>
        {
            var selfUrl = configuration["App:SelfUrl"];
            if (!string.IsNullOrWhiteSpace(selfUrl))
            {
                options.McpResource = selfUrl.TrimEnd('/') + "/mcp";
            }
        });

        context.Services.AddOptions<OpenIddictServerOptions>().Configure<IOptions<AbpAgentToolsOpenIddictOptions>>((server, options) =>
        {
            if (options.Value.RequireS256CodeChallengeMethod)
            {
                server.CodeChallengeMethods.Remove(OpenIddictConstants.CodeChallengeMethods.Plain);
            }

            // RFC 8707: spec-compliant MCP clients send resource=<endpoint URL>; OpenIddict refuses unknown ones (invalid_target).
            if (Uri.TryCreate(options.Value.McpResource, UriKind.Absolute, out var resource))
            {
                server.Resources.Add(resource);
            }

            server.Handlers.Add(AgentToolsScopeNarrowingHandler.Descriptor);
        });
        context.Services.TryAdd(AgentToolsScopeNarrowingHandler.Descriptor.ServiceDescriptor);

        context.Services.AddOptions<OpenIddictValidationOptions>().Configure<IOptions<AbpAgentToolsOpenIddictOptions>>((validation, options) =>
        {
            if (options.Value.EnableTokenEntryValidation)
            {
                validation.EnableTokenEntryValidation = true;
            }

            // The default scope resource is the MCP URL, so tokens carrying only the scope have aud=<MCP URL>. An empty list means
            // "any audience": adding to it would restrict the app, so only an app that restricts audiences gets it.
            if (options.Value.McpResource != null && validation.Audiences.Count > 0)
            {
                validation.Audiences.Add(options.Value.McpResource);
            }

            // ... and a token with only the scope is accepted at the MCP endpoint only, whatever the audience configuration.
            validation.Handlers.Add(AgentToolsMcpTokenAudienceHandler.Descriptor);
        });
        context.Services.TryAdd(AgentToolsMcpTokenAudienceHandler.Descriptor.ServiceDescriptor);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        // Web hosts only: a DbMigrator or another console host has no request pipeline (ABP sets the application builder for
        // web hosts) and initializes before it seeds, so the check would only warn that the scope is missing. The same holds
        // for a web host started to migrate (the single-layer template's --migrate-database, AddDataMigrationEnvironment).
        if (context.ServiceProvider.GetService<IObjectAccessor<IApplicationBuilder>>()?.Value == null
            || context.ServiceProvider.IsDataMigrationEnvironment())
        {
            return;
        }

        using var scope = context.ServiceProvider.CreateScope();
        await scope.ServiceProvider.GetRequiredService<AgentToolsOpenIddictStartupValidator>().ValidateAsync();
    }
}
