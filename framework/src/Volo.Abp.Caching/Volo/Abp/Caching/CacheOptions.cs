using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;

namespace Volo.Abp.Caching
{
    public class CacheOptions
    {
        /// <summary>
        /// Global Cache entry options.
        /// </summary>
        public DistributedCacheEntryOptions GlobalCacheEntryOptions { get; set; }

        /// <summary>
        /// List of all cache configurators.
        /// (func argument:Name of cache)
        /// </summary>
        public List<Func<string, DistributedCacheEntryOptions>> CacheConfigurators { get; set; } //TODO  list item use a configurator interface instead?

        public CacheOptions()
        {
            CacheConfigurators = new List<Func<string, DistributedCacheEntryOptions>>();
            GlobalCacheEntryOptions = new DistributedCacheEntryOptions();
        }
    }
}