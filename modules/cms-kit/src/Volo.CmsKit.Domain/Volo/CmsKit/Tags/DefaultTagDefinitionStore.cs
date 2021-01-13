using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
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

        public virtual Task<TagDefiniton> GetTagDefinitionAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            var result = options.Tags.GetOrDefault(entityType) ?? throw new EntityNotTaggableException(entityType);

            return Task.FromResult(result);
        }

        public virtual Task<List<TagDefiniton>> GetTagDefinitionsAsync()
        {
            return Task.FromResult(options.Tags.Values.ToList());
        }

        public virtual Task<bool> IsDefinedAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            return Task.FromResult(options.Tags.ContainsKey(entityType));
        }
    }
}
