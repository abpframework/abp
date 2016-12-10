using Volo.Abp.Domain.Entities;

namespace AbpDesk.Tickets
{
    public class Ticket : AggregateRoot<int>
    {
        public const int MaxTitleLength = 256;

        public const int MaxBodyLength = 64 * 1024; //64K

        public string Title { get; set; }

        public string Body { get; set; }

        private Ticket()
        {
            
        }

        public Ticket(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}
