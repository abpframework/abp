using JetBrains.Annotations;

namespace Volo.Abp.Caching
{
    public class CacheNameAttribute
    {
        public string Name { get; }

        public CacheNameAttribute([NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
        }
    }
}