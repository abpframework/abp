using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain
{
    public class Phone : Entity<long>
    {
        public virtual Guid PersonId { get; set; }

        public virtual string Number { get; set; }

        public virtual PhoneType Type { get; set; }

        private Phone()
        {
            
        }

        public Phone(Guid personId, string number, PhoneType type = PhoneType.Mobile)
        {
            PersonId = personId;
            Number = number;
            Type = type;
        }
    }
}