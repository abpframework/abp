using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Admin.Pages
{
    [Serializable]
    public class PageDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Title { get; set; }

        public string Slug { get; set; }
        
        public string Content { get; set; }

        public string Script { get; set; }

        public string Style { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}