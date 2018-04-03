using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.MongoDB
{
    public class MongoModelBuilder : IMongoModelBuilder
    {
        private readonly Dictionary<Type, MongoEntityModelBuilder> _entityModelBuilders;

        public MongoModelBuilder()
        {
            _entityModelBuilders = new Dictionary<Type, MongoEntityModelBuilder>();
        }

        public MongoDbContextModel Build()
        {
            var entityModels = _entityModelBuilders
                .Select(x => x.Value)
                .ToImmutableDictionary(x => x.EntityType, x => (IMongoEntityModel) x);

            return new MongoDbContextModel(entityModels);
        }

        public virtual void Entity<TEntity>([NotNull] Action<MongoEntityModelBuilder> buildAction)
        {
            Entity(typeof(TEntity), buildAction);
        }

        public virtual void Entity([NotNull] Type entityType, [NotNull] Action<MongoEntityModelBuilder> buildAction)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotNull(buildAction, nameof(buildAction));

            var model = _entityModelBuilders.GetOrAdd(entityType, () => new MongoEntityModelBuilder(entityType));
            buildAction(model);
        }

        public virtual IReadOnlyList<MongoEntityModelBuilder> GetEntities()
        {
            return _entityModelBuilders.Values.ToImmutableList();
        }
    }
}