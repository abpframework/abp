using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System;
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

        /// <summary>
        /// Gets single <see cref="TagEntityTypeDefiniton"/> by entityType.
        /// </summary>
        /// <param name="entityType">EntityType to get definition.</param>
        /// <exception cref="EntityNotTaggableException">Thrown when EntityType is not configured as taggable.</exception>
        /// <exception cref="InvalidOperationException">More than one element satisfies the condition in predicate.</exception>
        public virtual Task<TagEntityTypeDefiniton> GetTagEntityTypeDefinitionsAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            var result = options.EntityTypes.SingleOrDefault(x => x.EntityType == entityType) ?? throw new EntityNotTaggableException(entityType);

            return Task.FromResult(result);
        }

        /// <summary>
        /// Gets all defined <see cref="TagEntityTypeDefiniton"/> elements.
        /// </summary>
        public virtual Task<List<TagEntityTypeDefiniton>> GetTagEntityTypeDefinitionListAsync()
        {
            return Task.FromResult(options.EntityTypes.ToList());
        }

        /// <summary>
        /// Checks if EntityType defined as taggable.
        /// </summary>
        /// <param name="entityType">EntityType to check.</param>
        /// <exception cref="InvalidOperationException">More than one element satisfies the condition in predicate.</exception>"
        public virtual Task<bool> IsDefinedAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            var definition = options.EntityTypes.SingleOrDefault(x => x.EntityType == entityType);

            return Task.FromResult(definition != null);
        }
    }
}
