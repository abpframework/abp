using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public PostWithDetailsDto Post { get; set; }

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

        private async Task GetData()
        {
            Blog = await _blogAppService.GetByShortNameAsync(BlogShortName);
            Post = await _postAppService.GetByUrlAsync(new GetPostInput { BlogId = Blog.Id, Url = PostUrl });
            CommentsWithReplies = await _commentAppService.GetHierarchicalListOfPostAsync(new GetCommentListOfPostAsync() { PostId = Post.Id });
        }


        public class PostDetailsViewModel
        {
            public Guid? RepliedCommentId { get; set; }

            public Guid PostId { get; set; }

            public string Text { get; set; }
        }
    }
}