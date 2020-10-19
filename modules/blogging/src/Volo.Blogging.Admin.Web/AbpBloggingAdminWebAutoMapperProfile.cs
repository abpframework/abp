using AutoMapper;
using Volo.Blogging.Admin.Blogs;
using Volo.Blogging.Admin.Pages.Blogging.Admin.Blogs;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using EditModel = Volo.Blogging.Admin.Pages.Blogging.Admin.Blogs.EditModel;

namespace Volo.Blogging.Admin
{
    public class AbpBloggingAdminWebAutoMapperProfile : Profile
    {
        public AbpBloggingAdminWebAutoMapperProfile()
        {
            CreateMap<CreateModel.BlogCreateModalView, CreateBlogDto>();
            CreateMap<BlogDto, EditModel.BlogEditViewModel>();
        }
    }
}
