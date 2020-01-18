using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class DetailModel : BloggingPageModel
    {
        private const int TwitterLinkLength = 23;
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;
        private readonly ICommentAppService _commentAppService;

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

        public DetailModel(IPostAppService postAppService, IBlogAppService blogAppService, ICommentAppService commentAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
            _commentAppService = commentAppService;
        }

        public async Task OnGetAsync()
        {
            await GetData();
        }

        public async Task OnPostAsync()
        {
            var comment = await _commentAppService.CreateAsync(new CreateCommentDto()
            {
                RepliedCommentId = NewComment.RepliedCommentId,
                PostId = NewComment.PostId,
                Text = NewComment.Text
            });

            FocusCommentId = comment.Id;

            await GetData();
        }

        private async Task GetData()
        {
            Blog = await _blogAppService.GetByShortNameAsync(BlogShortName);
            Post = await _postAppService.GetForReadingAsync(new GetPostInput { BlogId = Blog.Id, Url = PostUrl });
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