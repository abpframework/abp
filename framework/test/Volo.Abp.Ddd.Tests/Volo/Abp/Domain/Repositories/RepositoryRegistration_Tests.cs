using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext), services);
            options.AddDefaultRepositories();

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories();

            //Assert

            //MyTestAggregateRootWithoutPk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));

            //MyTestAggregateRootWithGuidPk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));

            //MyTestEntityWithInt32Pk
            services.ShouldNotContainService(typeof(IReadOnlyRepository<MyTestEntityWithInt32Pk>));
            services.ShouldNotContainService(typeof(IBasicRepository<MyTestEntityWithInt32Pk>));
            services.ShouldNotContainService(typeof(IRepository<MyTestEntityWithInt32Pk>));
            services.ShouldNotContainService(typeof(IReadOnlyRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldNotContainService(typeof(IBasicRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldNotContainService(typeof(IRepository<MyTestEntityWithInt32Pk, int>));
        }

        [Fact]
        public void Should_Register_Default_Repositories_For_All_Entities()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext), services);
            options.AddDefaultRepositories(true);

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories();

            //Assert

            //MyTestAggregateRootWithoutPk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));

            //MyTestAggregateRootWithGuidPk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));

            //MyTestEntityWithInt32Pk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestEntityWithInt32Pk>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestEntityWithInt32Pk>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IReadOnlyBasicRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
        }

        [Fact]
        public void Should_Register_Custom_Repository()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext), services);
            options
                .AddDefaultRepositories(true)
                .AddRepository<MyTestAggregateRootWithGuidPk, MyTestAggregateRootWithDefaultPkCustomRepository>();

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories();

            //Assert

            //MyTestAggregateRootWithoutPk
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithoutPk>));

            //MyTestAggregateRootWithGuidPk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IReadOnlyBasicRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));

            //MyTestEntityWithInt32Pk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestEntityWithInt32Pk>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestEntityWithInt32Pk>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IReadOnlyBasicRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestDefaultRepository<MyTestEntityWithInt32Pk, int>));
        }

        [Fact]
        public void Should_Not_Register_Custom_Repository_If_Does_Not_Implement_Standard_Interfaces()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext), services);
            options
                .AddDefaultRepositories(true)
                .AddRepository<MyTestAggregateRootWithGuidPk, MyTestAggregateRootWithDefaultPkEmptyRepository>();

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories();

            //Assert

            services.ShouldNotContainService(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk>));
            services.ShouldNotContainService(typeof(IRepository<MyTestAggregateRootWithGuidPk>));
            services.ShouldNotContainService(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldNotContainService(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>));
        }

        [Fact]
        public void Should_Register_Default_Repositories_With_Custom_Base()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext), services);
            options
                .AddDefaultRepositories(true)
                .SetDefaultRepositoryClasses(typeof(MyTestCustomBaseRepository<,>), typeof(MyTestCustomBaseRepository<>));

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories();

            //Assert

            //MyTestAggregateRootWithoutPk
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithoutPk>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithoutPk>));

            //MyTestAggregateRootWithGuidPk
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestCustomBaseRepository<MyTestAggregateRootWithGuidPk, Guid>));

            //MyTestEntityWithInt32Pk
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestCustomBaseRepository<MyTestEntityWithInt32Pk, int>));
            services.ShouldContainTransient(typeof(IRepository<MyTestEntityWithInt32Pk, int>), typeof(MyTestCustomBaseRepository<MyTestEntityWithInt32Pk, int>));
        }

        [Fact]
        public void Should_Register_Default_Repository()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext), services);
            options.AddDefaultRepository<MyTestAggregateRootWithGuidPk>();

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories();

            //MyTestAggregateRootWithoutPk
            services.ShouldNotContainService(typeof(IReadOnlyRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldNotContainService(typeof(IBasicRepository<MyTestAggregateRootWithoutPk>));
            services.ShouldNotContainService(typeof(IRepository<MyTestAggregateRootWithoutPk>));

            //MyTestAggregateRootWithGuidPk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>));
        }

        [Fact]
        public void Should_Not_Register_Default_Repository_If_Registered_Custom_Repository()
        {
            //Arrange

            var services = new ServiceCollection();

            var options = new TestDbContextRegistrationOptions(typeof(MyFakeDbContext), services);
            options
                .AddDefaultRepository<MyTestAggregateRootWithGuidPk>()
                .AddRepository<MyTestAggregateRootWithGuidPk, MyTestAggregateRootWithDefaultPkCustomRepository>();;

            //Act

            new MyTestRepositoryRegistrar(options).AddRepositories();

            //MyTestAggregateRootWithGuidPk
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IReadOnlyRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IReadOnlyBasicRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IBasicRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
            services.ShouldContainTransient(typeof(IRepository<MyTestAggregateRootWithGuidPk, Guid>), typeof(MyTestAggregateRootWithDefaultPkCustomRepository));
        }

        public class MyTestRepositoryRegistrar : RepositoryRegistrarBase<AbpCommonDbContextRegistrationOptions>
        {
            public MyTestRepositoryRegistrar(AbpCommonDbContextRegistrationOptions options)
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
            public override object[] GetKeys()
            {
                return new object[] {MyId};
            }
        }

        public class MyTestDefaultRepository<TEntity> : RepositoryBase<TEntity>
            where TEntity : class, IEntity
        {

            [Obsolete("Use GetQueryableAsync method.")]
            protected override IQueryable<TEntity> GetQueryable()
            {
                throw new NotImplementedException();
            }

            public override Task<IQueryable<TEntity>> GetQueryableAsync()
            {
                throw new NotImplementedException();
            }

            public override Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public override Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public override Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public override Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public override Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public override Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public override Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public override Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false,
                CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        public class MyTestDefaultRepository<TEntity, TKey> : MyTestDefaultRepository<TEntity>, IRepository<TEntity, TKey>
            where TEntity : class, IEntity<TKey>
        {
            public Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        public class MyTestCustomBaseRepository<TEntity> : MyTestDefaultRepository<TEntity>
            where TEntity : class, IEntity
        {

        }

        public class MyTestCustomBaseRepository<TEntity, TKey> : MyTestDefaultRepository<TEntity, TKey>
            where TEntity : class, IEntity<TKey>
        {

        }

        public class MyTestAggregateRootWithDefaultPkCustomRepository : MyTestDefaultRepository<MyTestAggregateRootWithGuidPk, Guid>
        {

        }

        public interface IMyTestAggregateRootWithDefaultPkEmptyRepository : IRepository
        {

        }

        public class MyTestAggregateRootWithDefaultPkEmptyRepository : IMyTestAggregateRootWithDefaultPkEmptyRepository
        {

        }

        public class TestDbContextRegistrationOptions : AbpCommonDbContextRegistrationOptions
        {
            public TestDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
                : base(originalDbContextType, services)
            {
            }
        }
    }
}
