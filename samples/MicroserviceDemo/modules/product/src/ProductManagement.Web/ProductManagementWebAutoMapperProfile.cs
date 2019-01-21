using AutoMapper;
using ProductManagement.Pages.ProductManagement.Products;

namespace ProductManagement
{
    public class ProductManagementWebAutoMapperProfile : Profile
    {
        public ProductManagementWebAutoMapperProfile()
        {
            CreateMap<CreateModel.ProductCreateModalView, CreateProductDto>();
            CreateMap<ProductDto, EditModel.ProductEditModalView>();
        }
    }
}