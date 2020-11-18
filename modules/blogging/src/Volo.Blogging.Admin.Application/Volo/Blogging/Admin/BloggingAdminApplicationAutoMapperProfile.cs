using AutoMapper;
using Volo.Blogging.Admin.Blogs;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Admin
{
     public class BloggingAdminApplicationAutoMapperProfile : Profile
    {
        public BloggingAdminApplicationAutoMapperProfile()
        {
            CreateMap<Blog, BlogDto>();
        }
    }
}
