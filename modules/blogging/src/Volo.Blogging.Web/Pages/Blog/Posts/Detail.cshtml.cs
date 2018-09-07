using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommonMark;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class DetailModel : PageModel
    {
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

        public PostWithDetailsDto Post { get; set; }

        public IHtmlContent FormattedContent { get; set; }

        public IReadOnlyList<CommentWithRepliesDto> CommentsWithReplies { get; set; }

        public BlogDto Blog { get; set; }

        public DetailModel(IPostAppService postAppService, IBlogAppService blogAppService, ICommentAppService commentAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
            _commentAppService = commentAppService;
        }

        public async void OnGetAsync()
        {
            await GetData();
        }

        public async void OnPostAsync()
        {
            await _commentAppService.CreateAsync(new CreateCommentDto()
            {
                RepliedCommentId = NewComment.RepliedCommentId,
                PostId = NewComment.PostId,
                Text = NewComment.Text
            });

            await GetData();
        }

        public async void OnDeleteAsync(Guid commentId)
        {
            await _commentAppService.DeleteAsync(commentId);

            await GetData();
        }

        private async Task GetData()
        {
            Blog = await _blogAppService.GetByShortNameAsync(BlogShortName);
            Post = await _postAppService.GetForReadingAsync(new GetPostInput { BlogId = Blog.Id, Url = PostUrl });
            FormattedContent = RenderMarkdown(Post.Content);
            CommentsWithReplies = await _commentAppService.GetHierarchicalListOfPostAsync(new GetCommentListOfPostAsync() { PostId = Post.Id });
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

        public IHtmlContent RenderMarkdown(string content)
        {
            byte[] bytes = Encoding.Default.GetBytes(content);
            var utf8Content = Encoding.UTF8.GetString(bytes);

            var html = CommonMarkConverter.Convert(utf8Content);
            
            return new HtmlString(html);
        }

        public class PostDetailsViewModel
        {
            public Guid? RepliedCommentId { get; set; }

            public Guid PostId { get; set; }

            public string Text { get; set; }
        }
    }
}