﻿using AutoMapper;
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

namespace Volo.CmsKit.Admin
{
    public class CmsKitAdminApplicationAutoMapperProfile : Profile
    {
        public CmsKitAdminApplicationAutoMapperProfile()
        {
            CreateMap<CmsUser, Comments.CmsUserDto>();
            
            CreateMap<Comment, CommentDto>();
            CreateMap<Comment, CommentWithAuthorDto>()
                .Ignore(x=> x.Author);
            
            CreateMap<Page, PageDto>();

            CreateMap<BlogPost, BlogPostDto>(MemberList.Destination);
            CreateMap<CreateBlogPostDto, BlogPost>(MemberList.Source);
            CreateMap<UpdateBlogPostDto, BlogPost>(MemberList.Source);

            CreateMap<Blog, BlogDto>();

            CreateMap<TagEntityTypeDefiniton, TagDefinitionDto>(MemberList.Destination);

            CreateMap<MediaDescriptor, MediaDescriptorDto>();
        }
    }
}
