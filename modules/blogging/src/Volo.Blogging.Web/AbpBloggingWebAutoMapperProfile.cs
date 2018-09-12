using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Blogging.Blogs;
using Volo.Blogging.Pages.Blog.Posts;
using Volo.Blogging.Posts;
using IndexModel = Volo.Blogging.Pages.Blog.IndexModel;

namespace Volo.Blogging
{
    public class AbpBloggingWebAutoMapperProfile : Profile
    {
        public AbpBloggingWebAutoMapperProfile()
        {
            CreateMap<PostWithDetailsDto, EditPostViewModel>().Ignore(x=>x.Tags);
            CreateMap<NewModel.CreatePostViewModel, CreatePostDto>();
            CreateMap<IndexModel.BlogIndexViewModel, CreateBlogDto>();
        }
    }
}
