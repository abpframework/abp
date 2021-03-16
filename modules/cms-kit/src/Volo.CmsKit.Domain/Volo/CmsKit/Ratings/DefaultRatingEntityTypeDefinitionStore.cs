using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace Volo.CmsKit.Ratings
{
    public class DefaultRatingEntityTypeDefinitionStore : IRatingEntityTypeDefinitionStore
    {
        protected CmsKitRatingOptions Options { get; }

        public DefaultRatingEntityTypeDefinitionStore(IOptions<CmsKitRatingOptions> options)
        {
            Options = options.Value;
        }

        public virtual Task<RatingEntityTypeDefinition> GetAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            var definition = Options.EntityTypes.SingleOrDefault(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase)) ??
                         throw new EntityCantHaveRatingException(entityType);

            return Task.FromResult(definition);
        }

        public virtual Task<bool> IsDefinedAsync([NotNull] string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            var isDefined = Options.EntityTypes.Any(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(isDefined);
        }
    }
}
