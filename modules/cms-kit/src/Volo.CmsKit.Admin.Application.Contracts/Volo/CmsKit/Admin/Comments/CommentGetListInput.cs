using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Comments
{
    [Serializable]
    public class CommentGetListInput : PagedAndSortedResultRequestDto
    {
        public string EntityType { get; set; }

        public string Text { get; set; }

        public Guid? RepliedCommentId { get; set; }

        public string Author { get; set; }

        public DateTime? CreationStartDate { get; set; }
        
        public DateTime? CreationEndDate { get; set; }
    }
}