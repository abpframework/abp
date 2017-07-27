using AbpDesk.Tickets;
using AbpDesk.Tickets.Dtos;
using AutoMapper;

namespace AbpDesk
{
    public class AbpDeskApplicationModuleAutoMapperProfile : Profile
    {
        public AbpDeskApplicationModuleAutoMapperProfile()
        {
            CreateMap<Ticket, TicketDto>();
        }
    }
}