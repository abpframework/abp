using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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

            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));

            services.ShouldNotContainService(typeof(IRepository<MyTestEntityWithInt32Pk, int>));
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

            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
        }

        [Fact]
        public void Should_Register_Custom_Repository()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext));
            options
                .AddDefaultRepositories(true)
                .AddRepository<MyTestAggregateRootWithGuidPk, MyTestAggregateRootWithDefaultPkCustomRepository>();

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories(services);

            //Assert

            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
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

            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestCustomBaseRepository<MyTestEntityWithInt32Pk, int>));
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
                    typeof(MyTestEntityWithInt32Pk),
                    typeof(MyTestAggregateRootWithGuidPk),
                    typeof(MyTestAggregateRootWithoutPk)
                };
            }

            protected override Type GetRepositoryType(Type dbContextType, Type entityType)
            {
                return typeof(MyTestDefaultRepository<>).MakeGenericType(entityType);
            }

            protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
            {
                return typeof(MyTestDefaultRepository<,>).MakeGenericType(entityType, primaryKeyType);
            }
        }

        public class MyFakeDbContext { }

        public class MyTestAggregateRootWithGuidPk : AggregateRoot<Guid>
        {

        }

        public class MyTestEntityWithInt32Pk : Entity<int>
        {

        }

        public class MyTestAggregateRootWithoutPk : AggregateRoot
        {
            public string MyId { get; set; }
        }

        public class MyTestDefaultRepository<TEntity> : RepositoryBase<TEntity>
            where TEntity : class, IEntity
        {
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
        }

        public class MyTestDefaultRepository<TEntity, TPrimaryKey> : MyTestDefaultRepository<TEntity>, IRepository<TEntity, TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>
        {
            public TEntity Get(TPrimaryKey id)
            {
                throw new NotImplementedException();
            }

            public Task<TEntity> GetAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public TEntity Find(TPrimaryKey id)
            {
                throw new NotImplementedException();
            }

            public Task<TEntity> FindAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public void Delete(TPrimaryKey id)
            {
                throw new NotImplementedException();
            }

            public Task DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        public class MyTestAggregateRootWithDefaultPkCustomRepository : MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>
        {

        }

        public class MyTestCustomBaseRepository<TEntity> : MyTestDefaultRepository<TEntity>
            where TEntity : class, IEntity
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
