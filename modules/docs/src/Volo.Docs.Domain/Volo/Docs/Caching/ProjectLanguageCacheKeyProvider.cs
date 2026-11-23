using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Projects;

namespace Volo.Docs.Caching
{
    public class ProjectLanguageCacheKeyProvider : ITransientDependency
    {
        private readonly IDistributedCache<ProjectLanguageCacheGeneration> _cache;

        public ProjectLanguageCacheKeyProvider(IDistributedCache<ProjectLanguageCacheGeneration> cache)
        {
            _cache = cache;
        }

        public virtual async Task<string> GetKeyAsync(Project project, string version)
        {
            var generation = await _cache.GetOrAddAsync(project.Id.ToString("N"),
                () => Task.FromResult(CreateGeneration()), CreateOptions);

            return generation == null ? null : $"{CacheKeyGenerator.GenerateProjectLanguageCacheKey(project)}@{generation.Value}#{version}";
        }

        public virtual Task InvalidateAsync(Project project)
        {
            return _cache.SetAsync(project.Id.ToString("N"), CreateGeneration(), CreateOptions(), hideErrors: false);
        }

        private static ProjectLanguageCacheGeneration CreateGeneration()
        {
            return new ProjectLanguageCacheGeneration { Value = Guid.NewGuid().ToString("N") };
        }

        private static DistributedCacheEntryOptions CreateOptions()
        {
            return new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(2) };
        }
    }

    public class ProjectLanguageCacheGeneration
    {
        public string Value { get; set; }
    }
}
