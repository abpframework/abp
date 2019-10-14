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

    public class DummyObjectAsCacheKey
    {
        public string DummyData { get; set; }
        public int DummyInt { get; set; }
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