using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Menus
{
    [EventName("Volo.CmsKit.Menus.Updated")]
    public class MenuUpdatedEto : EtoBase
    {
        public Guid MenuId { get; set; }
    }
}