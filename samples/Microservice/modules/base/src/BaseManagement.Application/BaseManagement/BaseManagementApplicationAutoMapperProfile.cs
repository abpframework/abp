using AutoMapper;
using Volo.Abp.AutoMapper;

namespace BaseManagement
{
    public class BaseManagementApplicationAutoMapperProfile : Profile
    {
        public BaseManagementApplicationAutoMapperProfile()
        {
            CreateMap<BaseType, BaseTypeDto>();
            CreateMap<CreateBaseTypeDto, BaseType>().Ignore(x => x.BaseItems);
            CreateMap<UpdateBaseTypeDto, BaseType>();
        }
    }
}