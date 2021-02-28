using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Contents
{
    [Serializable]
    public class ContentGetListDto : EntityDto<Guid>
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }
    }
}
