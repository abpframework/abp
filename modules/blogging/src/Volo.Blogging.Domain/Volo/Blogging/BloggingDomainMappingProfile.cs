using AutoMapper;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;

namespace Volo.Blogging
{
    public class BloggingDomainMappingProfile : Profile
    {
        public BloggingDomainMappingProfile()
        {
            CreateMap<Blog, BlogEto>();
            CreateMap<Comment, CommentEto>();
            CreateMap<Post, PostEto>();
            CreateMap<Tag, TagEto>();
        }
    }
}