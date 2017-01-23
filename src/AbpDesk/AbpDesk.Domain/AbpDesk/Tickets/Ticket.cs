using System;
using JetBrains.Annotations;
using Volo;
using Volo.Abp.Domain.Entities;

namespace AbpDesk.Tickets
{
    public class Ticket : AggregateRoot<int>, IHasConcurrencyStamp
    {
        public const int MaxTitleLength = 256;

        public const int MaxBodyLength = 64 * 1024; //64K

        [NotNull]
        public string Title { get; protected set; }

        [CanBeNull]
        public string Body { get; protected set; }

        public string ConcurrencyStamp { get; set; }

        protected Ticket()
        {
            
        }

        public Ticket([NotNull] string title, [CanBeNull] string body = null)
        {
            Check.NotNull(title, nameof(title));

            Title = title;
            Body = body;
            ConcurrencyStamp = Guid.NewGuid().ToString();
        }
    }
}
