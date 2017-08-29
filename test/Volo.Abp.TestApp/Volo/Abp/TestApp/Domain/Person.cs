using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain
{
    public class Person : AggregateRoot
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}