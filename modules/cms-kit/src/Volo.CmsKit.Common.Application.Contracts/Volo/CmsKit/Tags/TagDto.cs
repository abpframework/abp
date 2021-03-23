using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Tags
{
    [Serializable]
    public class TagDto : EntityDto<Guid>
    {
        public string EntityType { get; set; }

        public string Name { get; set; }
    }
}
