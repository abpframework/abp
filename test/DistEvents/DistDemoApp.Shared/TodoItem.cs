using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DistDemoApp
{
    public class TodoItem : CreationAuditedAggregateRoot<Guid>
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, Text = {Text}";
        }
    }
}