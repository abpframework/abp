using System;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain
{
    public class Person : AggregateRoot
    {
        public virtual string Name { get; set; }

        public virtual int Age { get; set; }

        public virtual Collection<Phone> Phones { get; set; }

        private Person()
        {
            
        }

        public Person(Guid id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;

            Phones = new Collection<Phone>();
        }
    }
}