using AutoFilterer.Attributes;
using AutoFilterer.Types;
using Moq;
using Shouldly;
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

namespace Volo.Abp.AutoFilterer.Tests.Volo.Abp.AutoFilterer
{
    public class AbpFilterBase_Tests : AbpIntegratedTest<TestModule>
    {
        [Theory]
        [AutoMoqData(count: 32)]
        public async Task Should_Pagination_Work_Properly(
            Mock<IMemoryDatabaseProvider<TestDbContext>> mockProvider,
            List<Book> source)
        {
            // Arrange
            int skip = 3, take = 2;
            var query = source.AsQueryable();
            var repository = new MockingMemoryRepository<TestDbContext, Book, Guid>(mockProvider.Object, query);
            var sut = CreateAppService(repository);
            var filter = new BookFilterDto { SkipCount = skip, MaxResultCount = take };

            // Act
            var actual = await sut.GetListAsync(filter);
            var expected = query
                            .OrderByDescending(o => o.CreationTime)
                            .Skip(skip)
                            .Take(take)
                            .ToList();

            actual.ShouldNotBeNull();
            actual.Items.ShouldNotBeNull();
            actual.Items.Count.ShouldBe(expected.Count);
            for (int i = 0; i < actual.Items.Count; i++)
                actual.Items[i].Title.ShouldBe(expected[i].Title); // Id cannot be mocked. All ids are same.
        }

        [Theory]
        [AutoMoqData(count: 32)]
        public async Task Should_Sorting_Work_Properly(
            Mock<IMemoryDatabaseProvider<TestDbContext>> mockProvider,
            List<Book> source)
        {
            // Arrange
            string sorting = "Title DESC";
            var query = source.AsQueryable();
            var repository = new MockingMemoryRepository<TestDbContext, Book, Guid>(mockProvider.Object, query);
            var sut = CreateAppService(repository);
            var filter = new BookFilterDto { Sorting = sorting };

            // Act
            var actual = await sut.GetListAsync(filter);
            var expected = query
                            .OrderByDescending(x => x.Title)
                            .Skip(filter.SkipCount)
                            .Take(filter.MaxResultCount)
                            .ToList();

            actual.ShouldNotBeNull();
            actual.Items.ShouldNotBeNull();
            actual.Items.Count.ShouldBe(expected.Count);
            for (int i = 0; i < actual.Items.Count; i++)
                actual.Items[i].Title.ShouldBe(expected[i].Title); // Id cannot be mocked. All ids are same.
        }

        [Theory]
        [AutoMoqData(count: 32)]
        public async Task Should_Filter_Work_Properly(
            Mock<IMemoryDatabaseProvider<TestDbContext>> mockProvider,
            List<Book> source)
        {
            // Arrange
            string sorting = "Title DESC";
            string searchText = "ab";
            var query = source.AsQueryable();
            var repository = new MockingMemoryRepository<TestDbContext, Book, Guid>(mockProvider.Object, query);
            var sut = CreateAppService(repository);
            var filter = new BookFilterDto { Sorting = sorting, Filter = searchText };

            // Act
            var actual = await sut.GetListAsync(filter);
            var expected = query
                            // ToLowerContainsComparison attribute should generate exactly following expression:
                            .Where(x => x.Title.ToLowerInvariant().Contains(searchText.ToLowerInvariant()))
                            .OrderByDescending(x => x.Title)
                            .Skip(filter.SkipCount)
                            .Take(filter.MaxResultCount)
                            .ToList();

            actual.ShouldNotBeNull();
            actual.Items.ShouldNotBeNull();
            actual.Items.Count.ShouldBe(expected.Count);
            for (int i = 0; i < actual.Items.Count; i++)
                actual.Items[i].Title.ShouldBe(expected[i].Title); // Id cannot be mocked. All ids are same.
        }

        [Theory]
        [AutoMoqData(count: 32)]
        public async Task Should_RangeFilter_Work_Properly(
            Mock<IMemoryDatabaseProvider<TestDbContext>> mockProvider,
            List<Book> source)
        {
            // Arrange
            string sorting = "Title DESC";
            var query = source.AsQueryable();
            var repository = new MockingMemoryRepository<TestDbContext, Book, Guid>(mockProvider.Object, query);
            var sut = CreateAppService(repository);
            var filter = new BookFilterDto
            {
                Sorting = sorting,
                PublishDate = new Range<DateTime>()
                {
                    Min = DateTime.UtcNow
                }
            };

            // Act
            var actual = await sut.GetListAsync(filter);
            var expected = query
                            // TRange type should generate exactly following expression:
                            .Where(x => x.PublishDate >= filter.PublishDate.Min)
                            .OrderByDescending(x => x.Title)
                            .Skip(filter.SkipCount)
                            .Take(filter.MaxResultCount)
                            .ToList();

            actual.ShouldNotBeNull();
            actual.Items.ShouldNotBeNull();
            actual.Items.Count.ShouldBe(expected.Count);
            for (int i = 0; i < actual.Items.Count; i++)
                actual.Items[i].Title.ShouldBe(expected[i].Title); // Id cannot be mocked. All ids are same.
        }

        /* 
         * Rest of things are Autofilterer's job.
         * It has own tests, so all situations can't be tested here.
         * This test checks only if AutoFilterer breaks or not Abp defaults and if works together abp framework properly.
         */

        private BooksAppService CreateAppService(IRepository<Book, Guid> repository)
        {
            return new BooksAppService(repository) { ServiceProvider = this.ServiceProvider };
        }

        [PossibleSortings(typeof(BookDto))]
        public class BookFilterDto : AbpFilterBase
        {
            [CompareTo("Title")]
            [ToLowerContainsComparison]
            public string Filter { get; set; }

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
