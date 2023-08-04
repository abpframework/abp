using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Contents;
using Volo.CmsKit.GlobalResources;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Blogs;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.GlobalResources;
using Volo.CmsKit.Public.Ratings;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public;

public class PublicApplicationAutoMapperProfile : Profile
{
    public PublicApplicationAutoMapperProfile()
    {
        CreateMap<CmsUser, Comments.CmsUserDto>().MapExtraProperties();

        CreateMap<Comment, CommentDto>()
            .Ignore(x => x.Author).MapExtraProperties();

        CreateMap<Comment, CommentWithDetailsDto>()
            .Ignore(x => x.Replies)
            .Ignore(x => x.Author)
            .MapExtraProperties();

        CreateMap<Rating, RatingDto>();

        CreateMap<Page, PageCacheItem>().MapExtraProperties();

        CreateMap<PageCacheItem, PageDto>().MapExtraProperties();

        CreateMap<Page, PageDto>().MapExtraProperties();
        
        CreateMap<BlogPost, BlogPostCommonDto>().MapExtraProperties();

        CreateMap<MenuItem, MenuItemDto>().MapExtraProperties();

        CreateMap<GlobalResource, GlobalResourceDto>().MapExtraProperties();
    }
}
