using Volo.CmsKit.Users;

namespace Volo.CmsKit.Comments
{
    public class CommentWithAuthorQueryResultItem
    {
        public Comment Comment { get; set; }

        public CmsUser Author { get; set; }
    }
}
