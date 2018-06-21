using AutoMapper;
using Volo.Blogging.Blogs;

namespace Volo.Blogging
{
    public class BloggingApplicationAutoMapperProfile : Profile
    {
        public BloggingApplicationAutoMapperProfile()
        {
            CreateMap<Blog, BlogDto>();
        }
    }
}
