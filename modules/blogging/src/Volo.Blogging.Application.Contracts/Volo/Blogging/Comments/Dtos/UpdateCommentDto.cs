using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Blogging.Comments.Dtos
{
    public class UpdateCommentDto : IHasConcurrencyStamp
    {
        [Required]
        public string Text { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
