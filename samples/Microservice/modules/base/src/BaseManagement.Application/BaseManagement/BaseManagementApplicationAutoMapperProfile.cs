using AutoMapper;
using Volo.Abp.AutoMapper;

namespace BaseManagement
{
    public class BaseManagementApplicationAutoMapperProfile : Profile
    {
        public BaseManagementApplicationAutoMapperProfile()
        {
            CreateMap<BaseType, BaseTypeDto>();
            CreateMap<CreateUpdateBaseTypeDto, BaseType>().Ignore(x => x.BaseItems);

            CreateMap<BaseItem, BaseItemDto>();
            CreateMap<CreateUpdateBaseItemDto, BaseItem>().Ignore(x => x.BaseType);
        }
    }
}