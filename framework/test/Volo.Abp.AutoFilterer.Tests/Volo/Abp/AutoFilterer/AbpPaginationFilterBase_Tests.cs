using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Extensions;
using AutoFilterer.Swagger.OperationFilters;
using AutoFilterer.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AutoFilterer.Tests.Volo.Abp.TestBase;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MemoryDb;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AutoFilterer.Tests
{
    public class AbpPaginationFilterBase_Tests : AbpIntegratedTest<TestModule>
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
            int skip = 3, take = 2;
            var repository = new MockingMemoryRepository<TestDbContext, Book, Guid>(mockProvider.Object, source.AsQueryable());
            var sut = CreateAppService(repository);

            var filter = new BookFilterDto { SkipCount = skip, MaxResultCount = take };

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
            var sut =CreateAppService(repository);

            var filter = new BookFilterDto { Filter = searchText };

            // Act
            var actual = await sut.GetListAsync(filter);

            var expectedItems = queryable.ApplyFilter(filter).ToList();
            var expectedCount = filter.ApplyFilterWithoutPaginationAndOrdering(queryable).Count();

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
            var sut = CreateAppService(repository);

            var filter = new BookFilterDto { Sorting = sorting };

            // Act
            var actual = await sut.GetListAsync(filter);

            var expectedItems = queryable.ApplyFilter(filter).ToList();
            var expectedCount = filter.ApplyFilterWithoutPaginationAndOrdering(queryable).Count();

            // Assert
            actual.Items.ShouldNotBeNull();
            actual.TotalCount.ShouldBe(expectedCount);
            actual.Items.Count.ShouldBe(expectedItems.Count);

            for (int i = 0; i < actual.Items.Count; i++)
                actual.Items[i].Title.ShouldBe(expectedItems[i].Title);
        }

        private BooksAppService CreateAppService(IRepository<Book, Guid> repository)
        {
            return new BooksAppService(repository) { ServiceProvider = this.ServiceProvider };
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
    }
}
