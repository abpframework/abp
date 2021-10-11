using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Tags
{
    [Serializable]
    public class TagDto : EntityDto<Guid>, IHasConcurrencyStamp
    {
        public string EntityType { get; set; }

        public string Name { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
