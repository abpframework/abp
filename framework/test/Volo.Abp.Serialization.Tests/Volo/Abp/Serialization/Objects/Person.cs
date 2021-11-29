using System;

namespace Volo.Abp.Serialization.Objects
{
    [Serializable]
    public class Person
    {
        public string Name { get; private set; }

        private Person()
        {
            
        }

        public Person(string name)
        {
            Name = name;
        }
    }
}
