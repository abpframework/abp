using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public string BlogShortName { get; set; }

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
            var postDto = await _postAppService.GetAsync(new Guid(PostId));
            Post = ObjectMapper.Map<PostWithDetailsDto, EditPostViewModel>(postDto);
            Post.Tags = String.Join(", ", postDto.Tags.Select(p=>p.Name).ToArray());
        }

        public async Task<ActionResult> OnPost()
        {
            var post = new UpdatePostDto
            {
                BlogId = Post.BlogId,
                Title = Post.Title,
                Url = Post.Url,
                Content = Post.Content,
                Tags = Post.Tags
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
        public string Title { get; set; }

        [Required]
        [StringLength(PostConsts.MaxUrlLength)]
        public string Url { get; set; }

        [HiddenInput]
        [StringLength(PostConsts.MaxContentLength)]
        public string Content { get; set; }

        public string Tags { get; set; }
    }
}