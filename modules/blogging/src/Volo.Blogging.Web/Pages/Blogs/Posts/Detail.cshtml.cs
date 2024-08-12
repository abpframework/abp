using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Pages.Blogs.Shared.Helpers;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blogs.Posts
{
    public class DetailModel : BloggingPageModel
    {
        private const int TwitterLinkLength = 23;
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;
        private readonly ICommentAppService _commentAppService;
        private readonly BloggingUrlOptions _blogOptions;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string PostUrl { get; set; }

        [BindProperty]
        public PostDetailsViewModel NewComment { get; set; }

        public int CommentCount { get; set; }

        [HiddenInput]
        public Guid FocusCommentId { get; set; }

        public PostWithDetailsDto Post { get; set; }

        public IReadOnlyList<CommentWithRepliesDto> CommentsWithReplies { get; set; }

        public BlogDto Blog { get; set; }

        public List<PostWithDetailsDto> PostsList { get; set; }
        
        public IReadOnlyList<PostWithDetailsDto> LatestPosts { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string TagName { get; set; }

        public DetailModel(IPostAppService postAppService, IBlogAppService blogAppService, ICommentAppService commentAppService, IOptions<BloggingUrlOptions> blogOptions)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
            _commentAppService = commentAppService;
            _blogOptions = blogOptions.Value;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            if (BlogNameControlHelper.IsProhibitedFileFormatName(BlogShortName))
            {
                return NotFound();
            }
            
            if (_blogOptions.SingleBlogMode.Enabled)
            {
                BlogShortName = _blogOptions.SingleBlogMode.BlogName;
            }
            
            Blog = await GetBlogAsync(_blogAppService, _blogOptions, BlogShortName);
            if(Blog == null)
            {
                return NotFound();
            }

            await GetData();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var comment = await _commentAppService.CreateAsync(new CreateCommentDto()
            {
                RepliedCommentId = NewComment.RepliedCommentId,
                PostId = NewComment.PostId,
                Text = NewComment.Text
            });

            FocusCommentId = comment.Id;
            
            Blog = await GetBlogAsync(_blogAppService, _blogOptions, BlogShortName);
            if(Blog == null)
            {
                return NotFound();
            }

            await GetData();

            // PRG pattern
            // Redirect to the same page to prevent form resubmission
            return RedirectToPage();
        }

        private async Task GetData()
        {
            Post = await _postAppService.GetForReadingAsync(new GetPostInput { BlogId = Blog.Id, Url = PostUrl });
            
            PostsList = Post.Writer != null
                ? await _postAppService.GetListByUserIdAsync(Post.Writer.Id)
                : new List<PostWithDetailsDto>();

            LatestPosts = await _postAppService.GetLatestBlogPostsAsync(Blog.Id, 5);
            CommentsWithReplies = await _commentAppService.GetHierarchicalListOfPostAsync(Post.Id);
            CountComments();
        }

        public void CountComments()
        {
            CommentCount = CommentsWithReplies.Count;
            foreach (var commentWithReply in CommentsWithReplies)
            {
                CommentCount += commentWithReply.Replies.Count;
            }
        }

        public string GetTwitterShareUrl(string title, string url, string linkedAccounts)
        {
            var readAtString = " | Read More At ";
            var otherCharsLength = (readAtString + linkedAccounts).Length + 1;
            var maxTitleLength = 280 - TwitterLinkLength - otherCharsLength;
            title = title.Length < maxTitleLength ? title : title.Substring(0, maxTitleLength - 3) + "...";

            var text = title +
                       readAtString +
                       url +
                       " " + linkedAccounts;

            return (new UriBuilder("https://twitter.com/intent/tweet") { Query = "text=" + HttpUtility.UrlEncode(text) }).ToString();
        }

        public class PostDetailsViewModel
        {
            public Guid? RepliedCommentId { get; set; }

            public Guid PostId { get; set; }

            public string Text { get; set; }
        }
    }
}