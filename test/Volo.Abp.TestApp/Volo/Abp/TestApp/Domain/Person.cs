using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain
{
    public class Person : AggregateRoot
    {
        public string Name { get; set; }

        public int Age { get; set; }

        private Person()
        {
            
        }

        public Person(Guid id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }
    }
}