using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer
{
    public class IdentityServerAutoMapperProfile : Profile
    {
        public IdentityServerAutoMapperProfile()
        {
            //TODO: Reverse maps will not used probably. Remove those will not used

            CreateMap<Client, IdentityServer4.Models.Client>();

            CreateMap<ClientCorsOrigin, string>()
                .ConstructUsing(src => src.Origin)
                .ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

            CreateMap<ApiResource, IdentityServer4.Models.ApiResource>()
                .ForMember(dest => dest.ApiSecrets, opt => opt.MapFrom(src => src.Secrets));

            //TODO: Why PersistedGrant mapping is in this profile?
            CreateMap<PersistedGrant, IdentityServer4.Models.PersistedGrant>().ReverseMap();

            CreateMap<IdentityResource, IdentityServer4.Models.IdentityResource>();

            CreateMap<UserClaim, string>()
                .ConstructUsing(src => src.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<ApiSecret, IdentityServer4.Models.Secret>();

            CreateMap<ApiScope, IdentityServer4.Models.Scope>();

            CreateMap<ClientProperty, KeyValuePair<string, string>>()
              .ReverseMap();

            CreateMap<Client, IdentityServer4.Models.Client>()
                .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

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
        }
    }
}
