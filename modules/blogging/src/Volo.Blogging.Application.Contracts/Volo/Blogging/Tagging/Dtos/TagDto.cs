using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Blogging.Tagging.Dtos
{
    public class TagDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int UsageCount { get; set; }
    }
}