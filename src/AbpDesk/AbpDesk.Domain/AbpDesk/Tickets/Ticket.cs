using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo;
using Volo.Abp.Domain.Entities;

namespace AbpDesk.Tickets
{
    public class Ticket : AggregateRoot<int>, IHasConcurrencyStamp
    {
        public const int MaxTitleLength = 256;

        public const int MaxBodyLength = 64 * 1024; //64K

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; }

        [MaxLength(MaxBodyLength)]
        public string Body { get; set; }

        public string ConcurrencyStamp { get; set; }

        protected Ticket()
        {
            
        }

        public Ticket([NotNull] string title, string body)
        {
            Check.NotNull(title, nameof(title));

            Title = title;
            Body = body;
        }
    }
}
