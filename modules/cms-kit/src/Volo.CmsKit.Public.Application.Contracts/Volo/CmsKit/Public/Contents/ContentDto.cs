using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Contents
{
    public class ContentDto : EntityDto<Guid>
    {
        public string Value { get; set; }
    }
}
