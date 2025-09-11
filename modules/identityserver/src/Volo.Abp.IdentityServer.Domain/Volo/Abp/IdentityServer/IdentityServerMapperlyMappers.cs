using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientToISClientMapper : MapperBase<Client, IdentityServer4.Models.Client>
{
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.AllowedIdentityTokenSigningAlgorithms))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.Claims))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.ClientSecrets))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.AllowedGrantTypes))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.AllowedScopes))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.AllowedCorsOrigins))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.RedirectUris))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.PostLogoutRedirectUris))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.IdentityProviderRestrictions))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.Properties))]
    public override partial IdentityServer4.Models.Client Map(Client source);

    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.AllowedIdentityTokenSigningAlgorithms))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.Claims))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.ClientSecrets))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.AllowedGrantTypes))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.AllowedScopes))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.AllowedCorsOrigins))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.RedirectUris))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.PostLogoutRedirectUris))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.IdentityProviderRestrictions))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.Client.Properties))]
    public override partial void Map(Client source, IdentityServer4.Models.Client destination);

    public override void AfterMap(Client source, IdentityServer4.Models.Client destination)
    {
        destination.AllowedIdentityTokenSigningAlgorithms = AllowedSigningAlgorithmsConverter.SplitToArray(source.AllowedIdentityTokenSigningAlgorithms);
        if (source.Properties != null)
        {
            destination.Properties = source.Properties.ToDictionary(x => x.Key, x => x.Value);
        }
        if (source.Claims != null)
        {
            destination.Claims = source.Claims.Select(x => new IdentityServer4.Models.ClientClaim(x.Type, x.Value, ClaimValueTypes.String)).ToList();
        }
        if (source.ClientSecrets != null)
        {
            destination.ClientSecrets = source.ClientSecrets.Select(x => new IdentityServer4.Models.Secret(x.Value, x.Expiration) { Type = x.Type, Description = x.Description }).ToList();
        }
        if (source.AllowedGrantTypes != null)
        {
            destination.AllowedGrantTypes = source.AllowedGrantTypes.Select(x => x.GrantType).ToList();
        }
        if (source.AllowedScopes != null)
        {
            destination.AllowedScopes = source.AllowedScopes.Select(x => x.Scope).ToList();
        }
        if (source.AllowedCorsOrigins != null)
        {
            destination.AllowedCorsOrigins = source.AllowedCorsOrigins.Select(x => x.Origin).ToList();
        }
        if (source.RedirectUris != null)
        {
            destination.RedirectUris = source.RedirectUris.Select(x => x.RedirectUri).ToList();
        }
        if (source.PostLogoutRedirectUris != null)
        {
            destination.PostLogoutRedirectUris = source.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri).ToList();
        }
        if (source.IdentityProviderRestrictions != null)
        {
            destination.IdentityProviderRestrictions = source.IdentityProviderRestrictions.Select(x => x.Provider).ToList();
        }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ApiResourceToISApiResourceMapper : MapperBase<ApiResource, IdentityServer4.Models.ApiResource>
{
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.AllowedAccessTokenSigningAlgorithms))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.ApiSecrets))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.Properties))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.Scopes))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.UserClaims))]
    public override partial IdentityServer4.Models.ApiResource Map(ApiResource source);

    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.AllowedAccessTokenSigningAlgorithms))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.ApiSecrets))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.Properties))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.Scopes))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiResource.UserClaims))]
    public override partial void Map(ApiResource source, IdentityServer4.Models.ApiResource destination);

    public override void AfterMap(ApiResource source, IdentityServer4.Models.ApiResource destination)
    {
        destination.AllowedAccessTokenSigningAlgorithms = AllowedSigningAlgorithmsConverter.SplitToArray(source.AllowedAccessTokenSigningAlgorithms);
        if (source.Properties != null)
        {
            destination.Properties = source.Properties.ToDictionary(x => x.Key, x => x.Value);
        }
        if (source.Secrets != null)
        {
            destination.ApiSecrets = source.Secrets.Select(x => new IdentityServer4.Models.Secret(x.Value, x.Expiration) { Type = x.Type, Description = x.Description }).ToList();
        }
        if (source.UserClaims != null)
        {
            destination.UserClaims = source.UserClaims.Select(x => x.Type).ToList();
        }
        if (source.Scopes != null)
        {
            destination.Scopes = source.Scopes.Select(x => x.Scope).ToList();
        }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ApiScopeToISApiScopeMapper : MapperBase<ApiScope, IdentityServer4.Models.ApiScope>
{
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiScope.UserClaims))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiScope.Properties))]
    public override partial IdentityServer4.Models.ApiScope Map(ApiScope source);

    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiScope.UserClaims))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.ApiScope.Properties))]
    public override partial void Map(ApiScope source, IdentityServer4.Models.ApiScope destination);

    public override void AfterMap(ApiScope source, IdentityServer4.Models.ApiScope destination)
    {
        if (source.Properties != null)
        {
            destination.Properties = source.Properties.ToDictionary(x => x.Key, x => x.Value);
        }
        if (source.UserClaims != null)
        {
            destination.UserClaims = source.UserClaims.Select(x => x.Type).ToList();
        }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityResourceToISIdentityResourceMapper : MapperBase<IdentityResource, IdentityServer4.Models.IdentityResource>
{
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.IdentityResource.UserClaims))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.IdentityResource.Properties))]
    public override partial IdentityServer4.Models.IdentityResource Map(IdentityResource source);

    [MapperIgnoreTarget(nameof(IdentityServer4.Models.IdentityResource.UserClaims))]
    [MapperIgnoreTarget(nameof(IdentityServer4.Models.IdentityResource.Properties))]
    public override partial void Map(IdentityResource source, IdentityServer4.Models.IdentityResource destination);

    public override void AfterMap(IdentityResource source, IdentityServer4.Models.IdentityResource destination)
    {
        if (source.Properties != null)
        {
            destination.Properties = source.Properties.ToDictionary(x => x.Key, x => x.Value);
        }
        if (source.UserClaims != null)
        {
            destination.UserClaims = source.UserClaims.Select(x => x.Type).ToList();
        }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientToClientEtoMapper : MapperBase<Client, ClientEto>
{
    public override partial ClientEto Map(Client source);
    public override partial void Map(Client source, ClientEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityResourceToIdentityResourceEtoMapper : MapperBase<IdentityResource, IdentityResourceEto>
{
    public override partial IdentityResourceEto Map(IdentityResource source);
    public override partial void Map(IdentityResource source, IdentityResourceEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PersistedGrantToISPersistedGrantMapper : TwoWayMapperBase<PersistedGrant, IdentityServer4.Models.PersistedGrant>
{
    public override partial IdentityServer4.Models.PersistedGrant Map(PersistedGrant source);
    public override partial void Map(PersistedGrant source, IdentityServer4.Models.PersistedGrant destination);

    public override PersistedGrant ReverseMap(IdentityServer4.Models.PersistedGrant source)
    {
        var entity = new PersistedGrant(System.Guid.Empty)
        {
            Key = source.Key,
            Type = source.Type,
            SubjectId = source.SubjectId,
            SessionId = source.SessionId,
            ClientId = source.ClientId,
            Description = source.Description,
            CreationTime = source.CreationTime,
            Expiration = source.Expiration,
            ConsumedTime = source.ConsumedTime,
            Data = source.Data
        };
        return entity;
    }

    public override void ReverseMap(IdentityServer4.Models.PersistedGrant source, PersistedGrant destination)
    {
        destination.Key = source.Key;
        destination.Type = source.Type;
        destination.SubjectId = source.SubjectId;
        destination.SessionId = source.SessionId;
        destination.ClientId = source.ClientId;
        destination.Description = source.Description;
        destination.CreationTime = source.CreationTime;
        destination.Expiration = source.Expiration;
        destination.ConsumedTime = source.ConsumedTime;
        destination.Data = source.Data;
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PersistedGrantToPersistedGrantEtoMapper : MapperBase<PersistedGrant, PersistedGrantEto>
{
    public override partial PersistedGrantEto Map(PersistedGrant source);
    public override partial void Map(PersistedGrant source, PersistedGrantEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DeviceFlowCodesToDeviceFlowCodesEtoMapper : MapperBase<DeviceFlowCodes, DeviceFlowCodesEto>
{
    public override partial DeviceFlowCodesEto Map(DeviceFlowCodes source);
    public override partial void Map(DeviceFlowCodes source, DeviceFlowCodesEto destination);
}


