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
        private readonly CmsKitTagOptions options;

        public DefaultTagDefinitionStore(IOptions<CmsKitTagOptions> options)
        {
            this.options = options.Value;
        }

        public virtual Task<TagEntityTypeDefiniton> GetTagEntityTypeDefinitionsAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            var result = options.EntityTypes.GetOrDefault(entityType) ?? throw new EntityNotTaggableException(entityType);

            return Task.FromResult(result);
        }

        public virtual Task<List<TagEntityTypeDefiniton>> GetTagDefinitionsAsync()
        {
            return Task.FromResult(options.EntityTypes.Values.ToList());
        }

        public virtual Task<bool> IsDefinedAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            return Task.FromResult(options.EntityTypes.ContainsKey(entityType));
        }
    }
}
