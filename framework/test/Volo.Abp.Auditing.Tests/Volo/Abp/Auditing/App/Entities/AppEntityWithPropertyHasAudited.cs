using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities
{
    public class AppEntityWithPropertyHasAudited : AggregateRoot<Guid>
    {
        protected AppEntityWithPropertyHasAudited()
        {

        }

        public AppEntityWithPropertyHasAudited(Guid id, string name1)
            : base(id)
        {
            Name1 = name1;
        }

        [Audited]
        public string Name1 { get; set; }
        
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Name4 { get; set; }
        public string Name5 { get; set; }
        public string Name6 { get; set; }
        public string Name7 { get; set; }
        public string Name8 { get; set; }
        public string Name9 { get; set; }
        public string Name10 { get; set; }
        public string NameX { get; set; }

        [Audited] 
        public string Name99 { get; set; }
    }
}
