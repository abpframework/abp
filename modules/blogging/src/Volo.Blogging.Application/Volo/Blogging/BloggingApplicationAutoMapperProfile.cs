using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;
using Volo.Blogging.Users;

namespace Volo.Blogging
{
    public class BloggingApplicationAutoMapperProfile : Profile
    {
        public BloggingApplicationAutoMapperProfile()
        {
            CreateMap<Blog, BlogDto>();
            CreateMap<BlogUser, BlogUserDto>();
            CreateMap<Post, PostWithDetailsDto>().Ignore(x=>x.Writer).Ignore(x=>x.CommentCount).Ignore(x=>x.Tags);
            CreateMap<Comment, CommentWithDetailsDto>().Ignore(x => x.Writer);
            CreateMap<Tag, TagDto>();
            CreateMap<Post, PostCacheItem>().Ignore(x=>x.CommentCount).Ignore(x=>x.Tags);
            CreateMap<PostCacheItem, PostWithDetailsDto>()
                .IgnoreModificationAuditedObjectProperties()
                .IgnoreDeletionAuditedObjectProperties()
                .Ignore(x=>x.Writer)
                .Ignore(x => x.CommentCount)
                .Ignore(x => x.Tags);
        }
    }
}
