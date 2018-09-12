using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class NewModel : AbpPageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty]
        public CreatePostViewModel Post { get; set; }

        public BlogDto Blog { get; set; }

        public NewModel(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async Task OnGetAsync()
        {
            Blog = await _blogAppService.GetByShortNameAsync(BlogShortName);
            Post = new CreatePostViewModel
            {
                BlogId = Blog.Id
            };
        }

        public async Task<ActionResult> OnPost()
        {
            var blog = await _blogAppService.GetAsync(Post.BlogId);
            var postWithDetailsDto = await _postAppService.CreateAsync(ObjectMapper.Map<CreatePostViewModel,CreatePostDto>(Post));

            //TODO: Try Url.Page(...)
            return Redirect(Url.Content($"~/blog/{blog.ShortName}/{postWithDetailsDto.Url}"));
        }

        public class CreatePostViewModel
        {
            [Required]
            [HiddenInput]
            public Guid BlogId { get; set; }

            [Required]
            [StringLength(PostConsts.MaxTitleLength)]
            public string Title { get; set; }

            [Required]
            [HiddenInput]
            public string CoverImage { get; set; }

            [Required]
            [StringLength(PostConsts.MaxUrlLength)]
            public string Url { get; set; }

            [HiddenInput]
            [StringLength(PostConsts.MaxContentLength)]
            public string Content { get; set; }

            public string Tags { get; set; }
        }
    }
}