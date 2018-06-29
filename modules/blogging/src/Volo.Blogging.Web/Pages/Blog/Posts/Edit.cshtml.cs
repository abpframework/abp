using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class EditModel : AbpPageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;

        [BindProperty(SupportsGet = true)]
        public string PostId { get; set; }

        [BindProperty]
        public EditPostViewModel Post { get; set; }

        public EditModel(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async void OnGet()
        {
            var postDto = await _postAppService.GetForEditAsync(new Guid(PostId));
            Post = ObjectMapper.Map<GetPostForEditOutput, EditPostViewModel>(postDto);
        }

        public async Task<ActionResult> OnPost()
        {
            var post = new UpdatePostDto
            {
                BlogId = Post.BlogId,
                Title = Post.Title,
                Url = Post.Url,
                Content = Post.Content
            };

            var editedPost = await _postAppService.UpdateAsync(Post.Id, post);
            var blog = await _blogAppService.GetAsync(editedPost.BlogId);

            return Redirect(Url.Content($"~/blog/{blog.ShortName}/{editedPost.Url}"));
        }
    }

    public class EditPostViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [HiddenInput]
        public Guid BlogId { get; set; }

        [Required]
        [StringLength(PostConsts.MaxTitleLength)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(PostConsts.MaxUrlLength)]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [StringLength(PostConsts.MaxContentLength)]
        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}