using Volo.CmsKit.Users;

namespace Volo.CmsKit.Comments
{
    public class CommentWithAuthor
    {
        public Comment Comment { get; set; }

        public CmsUser Author { get; set; }
    }
}
