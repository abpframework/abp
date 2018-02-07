using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class SettingManager : ISettingManager, ISingletonDependency
    {
        protected Lazy<List<ISettingContributor>> Contributors { get; }

        protected SettingOptions Options { get; }

        public SettingManager(IOptions<SettingOptions> options, IServiceProvider serviceProvider)
        {
            Options = options.Value;

            Contributors = new Lazy<List<ISettingContributor>>(
                () => Options
                    .Contributors
                    .Select(c => serviceProvider.GetRequiredService(c) as ISettingContributor)
                    .ToList(),
                true
            );
        }

        public Task<string> GetOrNullAsync(string name)
        {
            return GetOrNullAsync(name, null, null);
        }

        public Task<string> GetOrNullAsync(string name, string entityType, string entityId, bool fallback = true)
        {
            foreach (var contributor in Contributors.Value)
            {
                var value = contributor.GetOrNull(name, entityType, entityId, fallback);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        public Task<List<SettingValue>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SettingValue>> GetAllAsync(string entityType, string entityId, bool fallback = true)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAsync(string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAsync(string name, string value, string entityType, string entityId)
        {
            throw new System.NotImplementedException();
        }
    }
}