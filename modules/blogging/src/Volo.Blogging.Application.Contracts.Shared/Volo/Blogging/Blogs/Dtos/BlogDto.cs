using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.Blogging.Blogs.Dtos
{
    public class BlogDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }
        
        public string ConcurrencyStamp { get; set; }
    }
}
