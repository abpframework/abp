using System;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Admin.Contents;

namespace Volo.CmsKit.Admin.Pages
{
    public class PageWithContentDto : EntityDto<Guid>
    {
        public string Title { get; set; }

        public string Url { get; set; }
        
        public string Description { get; set; }
        
        public ContentDto Content { get; set; }
    }
}