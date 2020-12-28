using System;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Pages
{
    public class PageWithContentDto : EntityDto<Guid>
    {
        public string Title { get; set; }

        public string Url { get; set; }
        
        public string Description { get; set; }
        
        public ContentDto Content { get; set; }
    }
}