using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.Comments;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Comments;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Admin.Menus;

namespace Volo.CmsKit.Admin;

public class CmsKitAdminApplicationAutoMapperProfile : Profile
{
    public CmsKitAdminApplicationAutoMapperProfile()
    {
        CreateMap<CmsUser, Comments.CmsUserDto>().MapExtraProperties();

        CreateMap<Comment, CommentDto>().MapExtraProperties();
        CreateMap<Comment, CommentWithAuthorDto>()
            .Ignore(x => x.Author)
            .MapExtraProperties();

        CreateMap<Page, PageDto>().MapExtraProperties();
        CreateMap<Page, PageLookupDto>();

        CreateMap<BlogPost, BlogPostDto>(MemberList.Destination).MapExtraProperties();
        CreateMap<BlogPost, BlogPostListDto>()
            .Ignore(d => d.BlogName)
            .MapExtraProperties();
        
        CreateMap<CreateBlogPostDto, BlogPost>(MemberList.Source).MapExtraProperties();
        CreateMap<UpdateBlogPostDto, BlogPost>(MemberList.Source).MapExtraProperties();

        CreateMap<Blog, BlogDto>().MapExtraProperties();

        CreateMap<TagEntityTypeDefiniton, TagDefinitionDto>(MemberList.Destination);

        CreateMap<Tag, TagDto>().MapExtraProperties();

        CreateMap<MediaDescriptor, MediaDescriptorDto>().MapExtraProperties();

        CreateMap<MenuItem, MenuItemDto>().MapExtraProperties();
        CreateMap<MenuItem, MenuItemWithDetailsDto>()
            .Ignore(x => x.PageTitle)
            .MapExtraProperties();
    }
}
