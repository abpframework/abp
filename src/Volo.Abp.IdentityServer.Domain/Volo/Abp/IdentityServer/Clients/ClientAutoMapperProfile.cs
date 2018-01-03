using AutoMapper;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientAutoMapperProfile : Profile
    {
        public ClientAutoMapperProfile()
        {
            CreateMap<Client, IdentityServer4.Models.Client>();

            CreateMap<ClientCorsOrigin, string>()
                .ConstructUsing(src => src.Origin)
                .ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));
        }
    }
}
