using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer;

public class IdentityServerAutoMapperProfile : Profile
{
    /// <summary>
    /// TODO: Reverse maps will not used probably. Remove those will not used
    /// </summary>
    public IdentityServerAutoMapperProfile()
    {
        CreateMap<UserClaim, string>()
            .ConstructUsing(src => src.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        CreateClientMap();
        CreateApiResourceMap();
        CreateApiScopeMap();
        CreateIdentityResourceMap();
        CreatePersistedGrantMap();
        CreateDeviceFlowCodesMap();
    }

    private void CreateClientMap()
    {
        CreateMap<ClientCorsOrigin, string>()
            .ConstructUsing(src => src.Origin)
            .ReverseMap()
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

        CreateMap<ClientProperty, KeyValuePair<string, string>>()
         .ReverseMap();

        CreateMap<Client, IdentityServer4.Models.Client>()
            .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
            .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedIdentityTokenSigningAlgorithms))
            .ReverseMap()
            .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedIdentityTokenSigningAlgorithms));

        CreateMap<ClientCorsOrigin, string>()
            .ConstructUsing(src => src.Origin)
            .ReverseMap()
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

        CreateMap<ClientIdPRestriction, string>()
            .ConstructUsing(src => src.Provider)
            .ReverseMap()
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

        CreateMap<ClientClaim, Claim>(MemberList.None)
            .ConstructUsing(src => new Claim(src.Type, src.Value))
            .ReverseMap();

        CreateMap<ClientClaim, IdentityServer4.Models.ClientClaim>(MemberList.None)
            .ConstructUsing(src => new IdentityServer4.Models.ClientClaim(src.Type, src.Value, ClaimValueTypes.String))
            .ReverseMap();

        CreateMap<ClientScope, string>()
            .ConstructUsing(src => src.Scope)
            .ReverseMap()
            .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

        CreateMap<ClientPostLogoutRedirectUri, string>()
            .ConstructUsing(src => src.PostLogoutRedirectUri)
            .ReverseMap()
            .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

        CreateMap<ClientRedirectUri, string>()
            .ConstructUsing(src => src.RedirectUri)
            .ReverseMap()
            .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

        CreateMap<ClientGrantType, string>()
            .ConstructUsing(src => src.GrantType)
            .ReverseMap()
            .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

        CreateMap<ClientSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
            .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
            .ReverseMap();

        CreateMap<Client, ClientEto>();
    }

    private void CreateApiResourceMap()
    {
        CreateMap<ApiResource, IdentityServer4.Models.ApiResource>()
            .ForMember(dest => dest.ApiSecrets, opt => opt.MapFrom(src => src.Secrets))
            .ForMember(x => x.AllowedAccessTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedAccessTokenSigningAlgorithms));

        CreateMap<ApiResourceSecret, IdentityServer4.Models.Secret>();

        CreateMap<ApiResourceScope, string>()
            .ConstructUsing(x => x.Scope)
            .ReverseMap()
            .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

        CreateMap<ApiResourceProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<ApiResource, ApiResourceEto>();
    }

    private void CreateApiScopeMap()
    {
        CreateMap<ApiScopeProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<ApiScopeClaim, string>()
            .ConstructUsing(x => x.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        CreateMap<ApiScope, IdentityServer4.Models.ApiScope>(MemberList.Destination)
            .ConstructUsing(src => new IdentityServer4.Models.ApiScope())
            .ReverseMap();
    }

    private void CreateIdentityResourceMap()
    {
        CreateMap<IdentityResource, IdentityServer4.Models.IdentityResource>()
            .ConstructUsing(src => new IdentityServer4.Models.IdentityResource());

        CreateMap<IdentityResourceClaim, string>()
            .ConstructUsing(x => x.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        CreateMap<IdentityResourceProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<IdentityResource, IdentityResourceEto>();
    }

    private void CreatePersistedGrantMap()
    {
        //TODO: Why PersistedGrant mapping is in this profile?
        CreateMap<PersistedGrant, IdentityServer4.Models.PersistedGrant>().ReverseMap();
        CreateMap<PersistedGrant, PersistedGrantEto>();
    }

    private void CreateDeviceFlowCodesMap()
    {
        CreateMap<DeviceFlowCodes, DeviceFlowCodesEto>();
    }
}
