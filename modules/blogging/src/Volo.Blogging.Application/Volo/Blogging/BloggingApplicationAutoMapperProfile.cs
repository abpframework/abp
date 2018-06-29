using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging
{
    public class BloggingApplicationAutoMapperProfile : Profile
    {
        public BloggingApplicationAutoMapperProfile()
        {
            CreateMap<Blog, BlogDto>();
            CreateMap<Post, PostDto>();
            CreateMap<Post, PostWithDetailsDto>();
            CreateMap<Post, GetPostForEditOutput>();
        }
    }
}
