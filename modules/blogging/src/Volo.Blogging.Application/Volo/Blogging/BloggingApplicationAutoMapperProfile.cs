using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging
{
    public class BloggingApplicationAutoMapperProfile : Profile
    {
        public BloggingApplicationAutoMapperProfile()
        {
            CreateMap<Blog, BlogDto>();
            CreateMap<Post, PostDto>();
            CreateMap<Post, PostWithDetailsDto>().Ignore(x=>x.CommentCount);
            CreateMap<Comment, CommentDto>();
            CreateMap<Tag, TagDto>();
        }
    }
}
