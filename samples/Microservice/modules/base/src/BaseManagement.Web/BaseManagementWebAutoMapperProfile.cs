using AutoMapper;
using BaseManagement.Pages.BaseManagement.Products;

namespace BaseManagement
{
    public class BaseManagementWebAutoMapperProfile : Profile
    {
        public BaseManagementWebAutoMapperProfile()
        {
            CreateMap<CreateModel.ProductCreateViewModel, CreateBaseTypeDto>();
            CreateMap<BaseTypeDto, EditModel.ProductEditViewModel>();
        }
    }
}