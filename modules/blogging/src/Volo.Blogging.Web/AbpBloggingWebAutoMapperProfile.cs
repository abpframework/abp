using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Volo.Blogging.Pages.Blog.Posts;
using Volo.Blogging.Posts;

namespace Volo.Blogging
{
    public class AbpBloggingWebAutoMapperProfile : Profile
    {
        public AbpBloggingWebAutoMapperProfile()
        {
            CreateMap<GetPostForEditOutput, EditPostViewModel>();
            CreateMap<NewModel.CreatePostViewModel, CreatePostDto>();
        }
    }
}
