using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Extensions;
using AutoFilterer.Swagger.OperationFilters;
using AutoFilterer.Types;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Moq;
using Shouldly;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Autofac;
using Volo.Abp.AutoFilterer.Tests.Volo.Abp.TestBase;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MemoryDb;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Linq;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AutoFilterer.Tests.Volo.Abp.AutoFilterer
{
    public class ApplicationService_AutoFilterer_Tests : AbpIntegratedTest<ApplicationService_AutoFilterer_Tests.TestModule>
    {
        [Fact]
        public void Should_Have_SwaggerOperatorFilters()
        {
            var options = ServiceProvider.GetService<IOptions<SwaggerGenOptions>>();

            options.Value.ShouldNotBeNull("Looks like swagger didn't initialized. Check test project.");

            options.Value.OperationFilterDescriptors.ShouldContain(x => x.Type == typeof(OrderableEnumOperationFilter), "OrderableEnumOperationFilter couldn't found. Make sure the method 'UseAutoFiltererParameters()' is called.");
            options.Value.OperationFilterDescriptors.ShouldContain(x => x.Type == typeof(InnerFilterPropertiesOperationFilter), "OrderableEnumOperationFilter couldn't found. Make sure the method 'UseAutoFiltererParameters()' is called.");
        }

        [Theory]
        [AutoMoqData(count: 64)]
        public async Task Should_Pagination_Works_Properly(
            Mock<IMemoryDatabaseProvider<TestDbContext>> mockProvider,
            List<Book> source)
        {
            // Arrange
            int count = 2;
            var repository = new MockingMemoryRepository<TestDbContext, Book, Guid>(mockProvider.Object, source.AsQueryable());
            var sut = new BooksAppService(repository) { ServiceProvider = this.ServiceProvider };

            var filter = new BookFilterDto { PerPage = count };

            // Act
            var actual = await sut.GetListAsync(filter);
            var expectedItems = source.AsQueryable().ApplyFilter(filter).ToList();
            var expectedCount = source.Count;

            // Assert
            actual.Items.ShouldNotBeNull();
            actual.TotalCount.ShouldBe(expectedCount);
            actual.Items.Count.ShouldBe(expectedItems.Count);
        }

        [Theory]
        [AutoMoqData(count: 64)]
        public async Task Should_Filter_Works_Properly(
            Mock<IMemoryDatabaseProvider<TestDbContext>> mockProvider,
            List<Book> source)
        {
            // Arrange
            string searchText = "a";
            var queryable = source.AsQueryable();
            var repository = new MockingMemoryRepository<TestDbContext, Book, Guid>(mockProvider.Object, queryable);
            var sut = new BooksAppService(repository) { ServiceProvider = this.ServiceProvider };

            var filter = new BookFilterDto { Filter = searchText };

            // Act
            var actual = await sut.GetListAsync(filter);

            var expectedItems = queryable.ApplyFilter(filter).ToList();
            var expectedCount = filter.ApplyFilterWithoutPagination(queryable).Count();

            // Assert
            actual.Items.ShouldNotBeNull();
            actual.TotalCount.ShouldBe(expectedCount);
            actual.Items.Count.ShouldBe(expectedItems.Count);
        }

        [Theory]
        [AutoMoqData(count: 64)]
        public async Task Should_Sorting_Works_Properly(
            Mock<IMemoryDatabaseProvider<TestDbContext>> mockProvider,
            List<Book> source)
        {
            // Arrange
            string sorting = nameof(Book.Title) + " DESC";
            var queryable = source.AsQueryable();
            var repository = new MockingMemoryRepository<TestDbContext, Book, Guid>(mockProvider.Object, queryable);
            var sut = new BooksAppService(repository) { ServiceProvider = this.ServiceProvider };

            var filter = new BookFilterDto { Sorting = sorting };

            // Act
            var actual = await sut.GetListAsync(filter);

            var expectedItems = queryable.ApplyFilter(filter).ToList();
            var expectedCount = filter.ApplyFilterWithoutPagination(queryable).Count();

            // Assert
            actual.Items.ShouldNotBeNull();
            actual.TotalCount.ShouldBe(expectedCount);
            actual.Items.Count.ShouldBe(expectedItems.Count);
        }

        [DependsOn(
            typeof(AbpAutofacModule),
            typeof(AbpSwashbuckleModule),
            typeof(AbpAutoMapperModule),
            typeof(AbpAutoFiltererModule)
            )]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                ConfigureSwaggerServices(context.Services);
                ConfigureAutoMapper();
                context.Services.AddTransient<IAsyncQueryableExecuter, AsyncQueryableExecuter>();
            }

            private void ConfigureSwaggerServices(IServiceCollection services)
            {
                services.AddSwaggerGen(
                    options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore API", Version = "v1" });
                        options.DocInclusionPredicate((docName, description) => true);
                        options.CustomSchemaIds(type => type.FullName);
                    }
                );
            }

            private void ConfigureAutoMapper()
            {
                Configure<AbpAutoMapperOptions>(options => options.AddMaps<TestModule>());
            }
        }
        public class TestDbContext : MemoryDbContext
        {
            public DbSet<Book> Books { get; set; }
        }
        public class Book : AuditedAggregateRoot<Guid>
        {
            public string Title { get; set; }
            public int TotalPage { get; set; }
            public DateTime PublishDate { get; set; }
        }

        [AutoMap(typeof(Book), ReverseMap = true)]
        public class BookDto : AuditedEntityDto<Guid>
        {
            public string Title { get; set; }
            public int TotalPage { get; set; }
            public DateTime PublishDate { get; set; }
        }

        [PossibleSortings(typeof(BookDto))]
        public class BookFilterDto : AbpPaginationFilterBase
        {
            public Guid[] Id { get; set; }

            [CompareTo("Title")]
            [StringFilterOptions(StringFilterOption.Contains)]
            public string Filter { get; set; }

            public Range<int> TotalPage { get; set; }

            public Range<DateTime> PublishDate { get; set; }
        }

        public class BooksAppService : CrudAutoFiltererAppService<Book, BookDto, Guid, BookFilterDto>
        {
            public BooksAppService(IRepository<Book, Guid> repository) : base(repository)
            {
            }
        }

        public class MockingMemoryRepository<TDbContext, TEntity, TKey> : MemoryDbRepository<TDbContext, TEntity, TKey>
            where TDbContext : MemoryDbContext
            where TEntity : class, IEntity<TKey>
        {
            private readonly IQueryable<TEntity> seed;

            public MockingMemoryRepository(IMemoryDatabaseProvider<TDbContext> databaseProvider, IQueryable<TEntity> seed) : base(databaseProvider)
            {
                this.seed = seed;
            }

            protected override IQueryable<TEntity> GetQueryable()
            {
                return seed;
            }
        }
    }
}
