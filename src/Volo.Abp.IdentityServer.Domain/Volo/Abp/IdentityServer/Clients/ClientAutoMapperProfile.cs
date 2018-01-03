using AutoMapper;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientAutoMapperProfile : Profile
    {
        public ClientAutoMapperProfile()
        {
            CreateMap<Client, IdentityServer4.Models.Client>();
        }
    }
}
