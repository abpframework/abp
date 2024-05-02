using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.GlobalResources;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Blogs;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.GlobalResources;
using Volo.CmsKit.Public.Pages;
using Volo.CmsKit.Public.Ratings;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public;

public class PublicApplicationAutoMapperProfile : Profile
{
    public PublicApplicationAutoMapperProfile()
    {
        CreateMap<CmsUser, Comments.CmsUserDto>();

        CreateMap<Comment, CommentDto>()
            .Ignore(x => x.Author);

        CreateMap<Comment, CommentWithDetailsDto>()
            .Ignore(x => x.Replies)
            .Ignore(x => x.Author);

        CreateMap<Rating, RatingDto>();

        CreateMap<Page, PageDto>();

        CreateMap<BlogPost, BlogPostPublicDto>(MemberList.None);

        CreateMap<MenuItem, MenuItemDto>();

        CreateMap<GlobalResource, GlobalResourceDto>();
    }
}
