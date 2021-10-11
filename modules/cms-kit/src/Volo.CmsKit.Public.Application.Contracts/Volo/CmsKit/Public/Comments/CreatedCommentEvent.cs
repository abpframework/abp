using System;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Public.Comments
{
    [EventName("Volo.CmsKit.Comments.Created")]
    public class CreatedCommentEvent
    {
        public Guid Id { get; set; }
    }
}
