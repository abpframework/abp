using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;

namespace Volo.Abp.OpenIddict;

public abstract class OpenIddictDataSeedContributorBase
{
    protected IConfiguration Configuration { get; }
    protected IOpenIddictApplicationRepository OpenIddictApplicationRepository { get; }
    protected IAbpApplicationManager ApplicationManager { get; }
    protected IOpenIddictScopeRepository OpenIddictScopeRepository { get; }
    protected IOpenIddictScopeManager ScopeManager { get; }

    public OpenIddictDataSeedContributorBase(
        IConfiguration configuration,
        IOpenIddictApplicationRepository openIddictApplicationRepository,
        IAbpApplicationManager applicationManager,
        IOpenIddictScopeRepository openIddictScopeRepository,
        IOpenIddictScopeManager scopeManager)
    {
        Configuration = configuration;
        OpenIddictApplicationRepository = openIddictApplicationRepository;
        ApplicationManager = applicationManager;
        OpenIddictScopeRepository = openIddictScopeRepository;
        ScopeManager = scopeManager;
    }

    protected virtual async Task CreateScopesAsync(OpenIddictScopeDescriptor scope)
    {
        if (await OpenIddictScopeRepository.FindByNameAsync(scope.Name) == null)
        {
            await ScopeManager.CreateAsync(scope);
        }
    }

    protected virtual async Task CreateOrUpdateApplicationAsync(
        [NotNull] string applicationType,
        [NotNull] string name,
        [NotNull] string type,
        [NotNull] string consentType,
        string displayName,
        string secret,
        List<string> grantTypes,
        List<string> scopes,
        List<string> redirectUris = null,
        List<string> postLogoutRedirectUris = null,
        string clientUri = null,
        string logoUri = null)
    {
        if (!string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
        {
            throw new AbpException("No client secret can be set for public applications.");
        }

        if (string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
        {
            throw new AbpException("The client secret is required for confidential applications.");
        }

        var application = new AbpApplicationDescriptor
        {
            ApplicationType = applicationType,
            ClientId = name,
            ClientType = type,
            ClientSecret = secret,
            ConsentType = consentType,
            DisplayName = displayName,
            ClientUri = clientUri,
            LogoUri = logoUri,
        };

        Check.NotNullOrEmpty(grantTypes, nameof(grantTypes));
        Check.NotNullOrEmpty(scopes, nameof(scopes));

        if (new[] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }.All(grantTypes.Contains))
        {
            application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);

            if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
            }
        }

        if (!redirectUris.IsNullOrEmpty() || !postLogoutRedirectUris.IsNullOrEmpty())
        {
            application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.EndSession);
        }

        var buildInGrantTypes = new[]
        {
            OpenIddictConstants.GrantTypes.Implicit, OpenIddictConstants.GrantTypes.Password,
            OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.ClientCredentials,
            OpenIddictConstants.GrantTypes.DeviceCode, OpenIddictConstants.GrantTypes.RefreshToken
        };

        foreach (var grantType in grantTypes)
        {
            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
            }

            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
            }

            if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                grantType == OpenIddictConstants.GrantTypes.ClientCredentials ||
                grantType == OpenIddictConstants.GrantTypes.Password ||
                grantType == OpenIddictConstants.GrantTypes.RefreshToken ||
                grantType == OpenIddictConstants.GrantTypes.DeviceCode)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
            }

            if (grantType == OpenIddictConstants.GrantTypes.ClientCredentials)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Password)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
            }

            if (grantType == OpenIddictConstants.GrantTypes.RefreshToken)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
            }

            if (grantType == OpenIddictConstants.GrantTypes.DeviceCode)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization);
            }

            if (grantType == OpenIddictConstants.GrantTypes.Implicit)
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
                if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                }
            }

            if (!buildInGrantTypes.Contains(grantType))
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.GrantType + grantType);
            }
        }

        var buildInScopes = new[]
        {
            OpenIddictConstants.Permissions.Scopes.Address,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles
        };

        foreach (var scope in scopes)
        {
            if (buildInScopes.Contains(scope))
            {
                application.Permissions.Add(scope);
            }
            else
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
            }
        }

        if (!redirectUris.IsNullOrEmpty())
        {
            foreach (var redirectUri in redirectUris!.Where(redirectUri => !redirectUri.IsNullOrWhiteSpace()))
            {
                if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                {
                    throw new AbpException("Invalid RedirectUri: " + redirectUri);
                }

                if (application.RedirectUris.All(x => x != uri))
                {
                    application.RedirectUris.Add(uri);
                }
            }
        }

        if (!postLogoutRedirectUris.IsNullOrEmpty())
        {
            foreach (var postLogoutRedirectUri in postLogoutRedirectUris!.Where(postLogoutRedirectUri => !postLogoutRedirectUri.IsNullOrWhiteSpace()))
            {
                if (!Uri.TryCreate(postLogoutRedirectUri, UriKind.Absolute, out var uri) ||
                    !uri.IsWellFormedOriginalString())
                {
                    throw new AbpException("Invalid PostLogoutRedirectUri: " + postLogoutRedirectUri);
                }

                if (application.PostLogoutRedirectUris.All(x => x != uri))
                {
                    application.PostLogoutRedirectUris.Add(uri);
                }
            }
        }

        var client = await OpenIddictApplicationRepository.FindByClientIdAsync(name);
        if (client == null)
        {
            await ApplicationManager.CreateAsync(application);

        }
        else
        {
            await ApplicationManager.UpdateAsync(client.ToModel(), application);
        }
    }

    protected virtual bool HasSameRedirectUris(OpenIddictApplication existingClient, AbpApplicationDescriptor application)
    {
        return existingClient.RedirectUris == JsonSerializer.Serialize(application.RedirectUris.Select(q => q.ToString().TrimEnd('/')));
    }

    protected virtual bool HasSameScopes(OpenIddictApplication existingClient, AbpApplicationDescriptor application)
    {
        return existingClient.Permissions == JsonSerializer.Serialize(application.Permissions.Select(q => q.ToString().TrimEnd('/')));
    }
}
