using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain
{
    public class Phone : Entity<long>
    {
        public virtual string Number { get; set; }

        public virtual PhoneType Type { get; set; }

        private Phone()
        {
            
        }

        public Phone(string number, PhoneType type = PhoneType.Mobile)
        {
            Number = number;
            Type = type;
        }
    }
}