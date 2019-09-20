using System;
using Shouldly;
using Xunit;

namespace Volo.Abp.Domain.Entities
{
    public class EntityHelper_Tests
    {
        [Fact]
        public static void SetId_DerivedFromAggregateRoot()
        {
            var idValue = Guid.NewGuid();
            var myEntityDerivedFromAggregateRoot = new MyEntityDerivedFromAggregateRoot();
            EntityHelper.TrySetId(myEntityDerivedFromAggregateRoot, () => idValue, true);
            myEntityDerivedFromAggregateRoot.Id.ShouldBe(idValue);
        }

        [Fact]
        public static void SetId_ImplementsIEntity()
        {
            var idValue = Guid.NewGuid();
            var myEntityImplementsIEntity = new MyEntityImplementsIEntity();
            EntityHelper.TrySetId(myEntityImplementsIEntity, () => idValue, true);
            myEntityImplementsIEntity.Id.ShouldBe(idValue);
        }

        [Fact]
        public static void SetId_DisablesIdGeneration()
        {
            var idValue = Guid.NewGuid();
            var myEntityDisablesIdGeneration = new MyEntityDisablesIdGeneration();
            EntityHelper.TrySetId(myEntityDisablesIdGeneration, () => idValue, true);
            myEntityDisablesIdGeneration.Id.ShouldBe(default);
        }

        private class MyEntityDerivedFromAggregateRoot : AggregateRoot<Guid>
        {

        }

        private class MyEntityImplementsIEntity : IEntity<Guid>
        {
            public Guid Id { get; protected set; }

            public object[] GetKeys()
            {
                return new object[] { Id };
            }
        }

        private class MyEntityDisablesIdGeneration : Entity<Guid>
        {
            [DisableIdGeneration]
            public override Guid Id { get; protected set; }
        }
    }
}
