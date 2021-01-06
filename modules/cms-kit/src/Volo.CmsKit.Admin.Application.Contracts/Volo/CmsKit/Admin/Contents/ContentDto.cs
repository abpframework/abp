using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Contents
{
    public class ContentDto : EntityDto<Guid>
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string Value { get; set; }
    }
}
