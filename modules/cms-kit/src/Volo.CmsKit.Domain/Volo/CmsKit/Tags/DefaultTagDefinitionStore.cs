using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Tags
{
    public class DefaultTagDefinitionStore : ITagDefinitionStore, ITransientDependency
    {
        private readonly CmsKitOptions options;

        public DefaultTagDefinitionStore(IOptions<CmsKitOptions> options)
        {
            this.options = options.Value;
        }

        public Task<TagDefiniton> GetTagDefinitionOrNullAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            return Task.FromResult(options.Tags.GetOrDefault(entityType));
        }

        public Task<List<TagDefiniton>> GetTagDefinitionsAsync()
        {
            return Task.FromResult(options.Tags.Values.ToList());
        }

        public Task<bool> IsDefinedAsync([NotNull] string entityType)
        {
            return Task.FromResult(options.Tags.ContainsKey(entityType));
        }
    }
}
