using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class ApiDescriptionCache : IApiDescriptionCache, ISingletonDependency
    {
        private readonly Dictionary<string, ApplicationApiDescriptionModel> _cache;
        private readonly SemaphoreSlim _semaphoreSlim;

        public ApiDescriptionCache()
        {
            _cache = new Dictionary<string, ApplicationApiDescriptionModel>();
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public async Task<ApplicationApiDescriptionModel> GetAsync(
            string baseUrl, 
            Func<Task<ApplicationApiDescriptionModel>> factory)
        {
            using (await _semaphoreSlim.LockAsync())
            {
                var model = _cache.GetOrDefault(baseUrl);
                if (model == null)
                {
                    _cache[baseUrl] = model = await factory();
                }

                return model;
            }
        }
    }
}