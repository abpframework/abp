using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Blogging.Pages.Blog.Posts;
using Volo.Blogging.Posts;

namespace Volo.Blogging
{
    public class AbpBloggingWebAutoMapperProfile : Profile
    {
        public AbpBloggingWebAutoMapperProfile()
        {
            CreateMap<PostWithDetailsDto, EditPostViewModel>().Ignore(x=>x.Tags);
            CreateMap<NewModel.CreatePostViewModel, CreatePostDto>();
        }
    }
}
