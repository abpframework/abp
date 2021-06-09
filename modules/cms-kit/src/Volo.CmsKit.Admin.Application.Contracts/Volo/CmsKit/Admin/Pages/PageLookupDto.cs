using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Pages
{
    public class PageLookupDto : EntityDto<Guid>
    {
        public string Title { get; set; }

        public string Slug { get; set; }
    }
}