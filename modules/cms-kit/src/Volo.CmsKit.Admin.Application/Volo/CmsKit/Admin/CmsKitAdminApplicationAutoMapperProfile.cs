using AutoMapper;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.Contents;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Contents;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin
{
    public class CmsKitAdminApplicationAutoMapperProfile : Profile
    {
        public CmsKitAdminApplicationAutoMapperProfile()
        {
            CreateMap<Page, PageDto>();

            CreateMap<Content, ContentDto>(MemberList.Destination);
            CreateMap<Content, ContentGetListDto>(MemberList.Destination);
            CreateMap<ContentCreateDto, Content>(MemberList.Source);
            CreateMap<ContentUpdateDto, Content>(MemberList.Source);

            CreateMap<BlogPost, BlogPostDto>(MemberList.Destination);
            CreateMap<CreateBlogPostDto, BlogPost>(MemberList.Source);
            CreateMap<UpdateBlogPostDto, BlogPost>(MemberList.Source);

            CreateMap<Blog, BlogDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<TagEntityTypeDefiniton, TagDefinitionDto>(MemberList.Destination);

            CreateMap<MediaDescriptor, MediaDescriptorDto>();
        }
    }
}
