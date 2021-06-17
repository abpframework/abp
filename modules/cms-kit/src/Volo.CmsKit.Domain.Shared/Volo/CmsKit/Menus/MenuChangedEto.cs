using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Menus
{
    [Serializable]
    [EventName("Volo.CmsKit.Menus.Changed")]
    public class MenuChangedEto : EtoBase
    {
        public Guid MenuId { get; set; }
    }
}