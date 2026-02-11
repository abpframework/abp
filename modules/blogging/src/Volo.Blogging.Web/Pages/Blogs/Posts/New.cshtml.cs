using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Validation;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Pages.Blogs.Shared.Helpers;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blogs.Posts
{
    public class NewModel : BloggingPageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;
        private readonly IAuthorizationService _authorization;
        private readonly BloggingUrlOptions _blogOptions;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty]
        public CreatePostViewModel Post { get; set; }

        public BlogDto Blog { get; set; }

        public NewModel(IPostAppService postAppService, IBlogAppService blogAppService, IAuthorizationService authorization, IOptions<BloggingUrlOptions> blogOptions)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
            _authorization = authorization;
            _blogOptions = blogOptions.Value;
        }

        public virtual async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Posts.Create))
            {
                return Redirect("/");
            }
            if (BlogNameControlHelper.IsProhibitedFileFormatName(BlogShortName))
            {
                return NotFound();
            }

            Blog = await GetBlogAsync(_blogAppService, _blogOptions, BlogShortName);
            BlogShortName = Blog.ShortName;
            
            Post = new CreatePostViewModel
            {
                BlogId = Blog.Id
            };

            return Page();
        }

        public virtual async Task<ActionResult> OnPost()
        {
            var blog = await _blogAppService.GetAsync(Post.BlogId);

            if (string.IsNullOrEmpty(Post.Description))
            {
                Post.Description = Post.Content.Truncate(PostConsts.MaxSeoFriendlyDescriptionLength);
            }

            Post.Url = WebUtility.UrlEncode(Post.Url);
            
            var postWithDetailsDto = await _postAppService.CreateAsync(ObjectMapper.Map<CreatePostViewModel, CreatePostDto>(Post));

            Dictionary<string, object> routeValues = new()
            {
                { nameof(DetailModel.PostUrl), postWithDetailsDto.Url }
            };

            if (!_blogOptions.SingleBlogMode.Enabled)
            {
                routeValues.Add(nameof(DetailModel.BlogShortName), blog.ShortName);
            }
            
            return RedirectToPage("/Blogs/Posts/Detail", routeValues);
        }

        public class CreatePostViewModel
        {
            [Required]
            [HiddenInput]
            public Guid BlogId { get; set; }

            [Required]
            [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxTitleLength))]
            public string Title { get; set; }

            [Required]
            [HiddenInput]
            public string CoverImage { get; set; }

            [Required]
            [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxUrlLength))]
            public string Url { get; set; }

            [Required]
            [HiddenInput]
            [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxContentLength))]
            public string Content { get; set; }

            public string Tags { get; set; }

            [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxDescriptionLength))]
            public string Description { get; set; }

        }
    }
}
