using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Blogging.Admin.Blogs
{
    public class UpdateBlogDto : IHasConcurrencyStamp
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

        public string Description { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
