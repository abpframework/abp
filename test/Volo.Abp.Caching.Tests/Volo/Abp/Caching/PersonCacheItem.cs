using System;

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
}