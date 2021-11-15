using System;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.Domain.Entities
{
    public class Entity_Tests
    {
        [Fact]
        public void EntityEquals_Should_Return_True_For_Same_Keys()
        {
            var idValue1 = Guid.NewGuid();
            var idValue2 = Guid.NewGuid();
            
            new Person(idValue1).EntityEquals(new Person(idValue1)).ShouldBeTrue();
            
            new Car(42).EntityEquals(new Car(42)).ShouldBeTrue();
            
            new Product("a").EntityEquals(new Product("a")).ShouldBeTrue();

            new Phone(idValue1, "123").EntityEquals(new Phone(idValue1, "123")).ShouldBeTrue();
        }
        
        [Fact]
        public void EntityEquals_Should_Return_False_For_Different_Keys()
        {
            var idValue1 = Guid.NewGuid();
            var idValue2 = Guid.NewGuid();
            
            new Person(idValue1).EntityEquals(new Person()).ShouldBeFalse();
            new Person(idValue1).EntityEquals(new Person(idValue2)).ShouldBeFalse();
            
            new Car(42).EntityEquals(new Car()).ShouldBeFalse();
            new Car(42).EntityEquals(new Car(43)).ShouldBeFalse();
            
            new Product("a").EntityEquals(new Product()).ShouldBeFalse();
            new Product("a").EntityEquals(new Product("b")).ShouldBeFalse();
            
            new Phone(idValue1, "123").EntityEquals(new Phone()).ShouldBeFalse();
            new Phone(idValue1, "123").EntityEquals(new Phone(idValue1, null)).ShouldBeFalse();
            new Phone(idValue1, "123").EntityEquals(new Phone(idValue1, "321")).ShouldBeFalse();
        }
        
        [Fact]
        public void EntityEquals_Should_Return_False_For_Both_Default_Keys()
        {
            new Person().EntityEquals(new Person()).ShouldBeFalse();
            
            new Car().EntityEquals(new Car()).ShouldBeFalse();
            
            new Product().EntityEquals(new Product()).ShouldBeFalse();
            
            new Phone().EntityEquals(new Phone()).ShouldBeFalse();
        }

        [Fact]
        public void Different_Tenants_With_Same_Keys_Considered_As_Different_Objects()
        {
            var tenantId1 = Guid.NewGuid();
            var tenantId2 = Guid.NewGuid();
            
            new Car(42, tenantId1).EntityEquals(new Car(42)).ShouldBeFalse();
            new Car(42).EntityEquals(new Car(42, tenantId2)).ShouldBeFalse();
            new Car(42, tenantId1).EntityEquals(new Car(42, tenantId2)).ShouldBeFalse();
        }
        
        [Fact]
        public void Same_Tenants_With_Same_Keys_Considered_As_Same_Objects()
        {
            var tenantId1 = Guid.NewGuid();
            
            new Car(42).EntityEquals(new Car(42)).ShouldBeTrue();
            new Car(42, tenantId1).EntityEquals(new Car(42, tenantId1)).ShouldBeTrue();
        }

        [Fact]
        public void Should_Set_TenantId_On_Object_Creation()
        {
            var tenantId = Guid.NewGuid();
            AsyncLocalCurrentTenantAccessor.Instance.Current = new BasicTenantInfo(tenantId);
            new Car().TenantId.ShouldBe(tenantId);
        }
        
        [Fact]
        public void Should_Override_TenantId_Manually()
        {
            AsyncLocalCurrentTenantAccessor.Instance.Current = null;
            
            var tenantId = Guid.NewGuid();
            new Car(0, tenantId).TenantId.ShouldBe(tenantId);
        }

        public class Person : Entity<Guid>
        {
            public Person()
            {
            }

            public Person(Guid id)
                : base(id)
            {
            }
        }

        public class Car : Entity<int>, IMultiTenant
        {
            public Guid? TenantId { get; private set; }

            public Car()
            {
            }

            public Car(int id, Guid? tenantId = null)
                : base(id)
            {
                TenantId = tenantId;
            }
        }

        public class Product : Entity<string>
        {
            public Product()
            {
            }

            public Product(string id)
                : base(id)
            {
            }
        }

        public class Phone : Entity
        {
            public Guid PersonId { get; set; }

            public string Number { get; set; }

            public Phone()
            {
                
            }

            public Phone(Guid personId, string number)
            {
                PersonId = personId;
                Number = number;
            }

            public override object[] GetKeys()
            {
                return new Object[] {PersonId, Number};
            }
        }
    }
}