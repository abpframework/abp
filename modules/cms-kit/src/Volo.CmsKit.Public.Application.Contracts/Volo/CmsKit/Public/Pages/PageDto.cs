using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Pages
{
    public class PageDto : EntityDto<Guid>
    {
        public string Title { get; set; }

        public string Url { get; set; }
    }
}