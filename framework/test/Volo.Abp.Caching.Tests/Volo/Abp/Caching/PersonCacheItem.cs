using System;
using System.Linq;

namespace Volo.Abp.Caching
{
    [Serializable]
    public class PersonCacheItem
    {
        public string Name { get; private set; }

        private PersonCacheItem()
        {

        }

        public PersonCacheItem(string name)
        {
            Name = name;
        }
    }

    public class ComplexObjectAsCacheKey
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            // Return selective fields
            //return $"{Name}_{Age}";
            // Return all the fields concatenated
            var sb = new System.Text.StringBuilder();
            var properties = this.GetType().GetProperties()
                .Where(prop => prop.CanRead && prop.CanWrite);
            foreach (var prop in properties)
            {
                var value = prop.GetValue(this, null);
                if (value != null)
                {
                    sb.Append(value.ToString());
                }
            }
            return sb.ToString();
        }
    }
}

namespace Sail.Testing.Caching
{
    [Serializable]
    public class PersonCacheItem
    {
        public string Name { get; private set; }

        private PersonCacheItem()
        {

        }

        public PersonCacheItem(string name)
        {
            Name = name;
        }
    }
}