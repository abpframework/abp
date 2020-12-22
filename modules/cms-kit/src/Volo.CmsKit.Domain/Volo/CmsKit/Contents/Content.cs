using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.CmsKit.Domain.Shared.Volo.CmsKit.Contents;

namespace Volo.CmsKit.Domain.Volo.CmsKit.Contents
{
    public class Content : FullAuditedAggregateRoot<Guid>
    {
        public string Value { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
    }
}
