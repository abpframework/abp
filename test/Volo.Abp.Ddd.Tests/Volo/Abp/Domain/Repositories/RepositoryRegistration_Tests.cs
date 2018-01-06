using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace Volo.Abp.Domain.Repositories
{
    public class RepositoryRegistration_Tests
    {
        [Fact]
        public void Should_Register_Default_Repositories_For_AggregateRoots()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext));
            options.AddDefaultRepositories();

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories(services);

            //Assert

            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithDefaultPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithDefaultPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithDefaultPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithDefaultPk>));
            services.ShouldNotContainService(typeof(IRepository<MyTestEntityWithCustomPk, int>));
        }

        [Fact]
        public void Should_Register_Default_Repositories_For_All_Entities()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext));
            options.AddDefaultRepositories(true);

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories(services);

            //Assert

            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithDefaultPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithDefaultPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithDefaultPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithDefaultPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithCustomPk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithCustomPk, int>));
        }

        [Fact]
        public void Should_Register_Custom_Repository()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext));
            options
                .AddDefaultRepositories(true)
                .AddCustomRepository<MyTestAggregateRootWithDefaultPk, MyTestAggregateRootWithDefaultPkCustomRepository>();

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories(services);

            //Assert

            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithDefaultPk>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithDefaultPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithCustomPk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithCustomPk, int>));
        }

        [Fact]
        public void Should_Register_Default_Repositories_With_Custom_Base()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext));
            options
                .AddDefaultRepositories(true)
                .SetDefaultRepositoryClasses(typeof(MyTestCustomBaseRepository<,>), typeof(MyTestCustomBaseRepository<>));

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories(services);

            //Assert

            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithDefaultPk>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithDefaultPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithDefaultPk, Guid>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithDefaultPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithCustomPk, int>), typeof(MyTestCustomBaseRepository<MyTestEntityWithCustomPk, int>));
        }

        public class MyTestRepositoryRegistrar : RepositoryRegistrarBase<CommonDbContextRegistrationOptions>
        {
            public MyTestRepositoryRegistrar(CommonDbContextRegistrationOptions options) 
                : base(options)
            {
            }

            protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
            {
                return new[]
                {
                    typeof(MyTestEntityWithCustomPk),
                    typeof(MyTestAggregateRootWithDefaultPk)
                };
            }

            protected override Type GetRepositoryTypeForDefaultPk(Type dbContextType, Type entityType)
            {
                return typeof(MyTestDefaultRepository<>).MakeGenericType(entityType);
            }

            protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
            {
                return typeof(MyTestDefaultRepository<,>).MakeGenericType(entityType, primaryKeyType);
            }
        }

        public class MyFakeDbContext { }

        public class MyTestAggregateRootWithDefaultPk : AggregateRoot
        {
            
        }

        public class MyTestEntityWithCustomPk : Entity<int>
        {

        }

        public class MyTestDefaultRepository<TEntity> : MyTestDefaultRepository<TEntity, Guid>, IRepository<TEntity>
            where TEntity : class, IEntity<Guid>
        {
            
        }

        public class MyTestDefaultRepository<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>
        {
            public override TEntity Find(TPrimaryKey id)
            {
                throw new NotImplementedException();
            }

            public override TEntity Insert(TEntity entity, bool autoSave = false)
            {
                throw new NotImplementedException();
            }

            public override TEntity Update(TEntity entity)
            {
                throw new NotImplementedException();
            }

            public override void Delete(TEntity entity)
            {
                throw new NotImplementedException();
            }

            public override long GetCount()
            {
                throw new NotImplementedException();
            }

            public override List<TEntity> GetList()
            {
                throw new NotImplementedException();
            }
        }

        public class MyTestAggregateRootWithDefaultPkCustomRepository : MyTestDefaultRepository<MyTestAggregateRootWithDefaultPk>
        {

        }

        public class MyTestCustomBaseRepository<TEntity> : MyTestCustomBaseRepository<TEntity, Guid>, IRepository<TEntity>
            where TEntity : class, IEntity<Guid>
        {

        }

        public class MyTestCustomBaseRepository<TEntity, TPrimaryKey> : MyTestDefaultRepository<TEntity, TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>
        {

        }

        public class TestDbContextRegistrationOptions : CommonDbContextRegistrationOptions
        {
            public TestDbContextRegistrationOptions(Type originalDbContextType) 
                : base(originalDbContextType)
            {
            }
        }
    }
}
