using System;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TestApp.Domain
{
    public class Person : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        public virtual Guid? CityId { get; set; }

        public virtual string Name { get; private set; }

        public virtual int Age { get; set; }

        public virtual Collection<Phone> Phones { get; set; }

        private Person()
        {
            
        }

        public Person(Guid id, string name, int age, Guid? tenantId = null, Guid? cityId = null)
            : base(id)
        {
            Name = name;
            Age = age;
            TenantId = tenantId;
            CityId = cityId;

            Phones = new Collection<Phone>();
        }

        public virtual void ChangeName(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var oldName = Name;
            Name = name;

            AddLocalEvent(
                new PersonNameChangedEvent
                {
                    Person = this,
                    OldName = oldName
                }
            );

            AddDistributedEvent(
                new PersonNameChangedEto
                {
                    Id = Id,
                    OldName = oldName,
                    NewName = Name,
                    TenantId = TenantId
                }
            );
        }
    }
}