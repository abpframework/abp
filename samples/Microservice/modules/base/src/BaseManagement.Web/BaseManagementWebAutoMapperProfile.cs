using AutoMapper;
using BaseManagement.Pages.BaseManagement.Products;

namespace BaseManagement
{
    public class BaseManagementWebAutoMapperProfile : Profile
    {
        public BaseManagementWebAutoMapperProfile()
        {
            CreateMap<CreateModel.ProductCreateViewModel, CreateUpdateBaseTypeDto>();
            CreateMap<BaseTypeDto, EditModel.ProductEditViewModel>();
        }
    }
}