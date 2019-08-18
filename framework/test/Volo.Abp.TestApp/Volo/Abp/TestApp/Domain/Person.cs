using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TestApp.Domain
{
    [AutoMapTo(typeof(PersonEto))]
    public class Person : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        public virtual Guid? CityId { get; set; }

        public virtual string Name { get; protected set; }

        public virtual int Age { get; set; }

        public virtual IList<Phone> Phones { get; set; }

        protected Person()
        {
            
        }

        public Person(Guid id, string name, int age, Guid? tenantId = null, Guid? cityId = null)
        {
            Id = id;
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