using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Acme.BookStore
{
    public class BookStoreTestDataBuilder : ITransientDependency
    {
        private readonly IDataSeeder _dataSeeder;
        private readonly IRepository<Book, Guid> _bookRepository;

        public BookStoreTestDataBuilder(IDataSeeder dataSeeder, IRepository<Book, Guid> bookRepository)
        {
            _dataSeeder = dataSeeder;
            _bookRepository = bookRepository;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildInternalAsync);
        }

        public async Task BuildInternalAsync()
        {
            await _dataSeeder.SeedAsync();
            await _bookRepository.InsertAsync(
                new Book
                {
                    Id = Guid.NewGuid(),
                    Name = "Test book 1",
                    Type = BookType.Fantastic,
                    PublishDate = new DateTime(2015, 05, 24),
                    Price = 21
                }
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    Id = Guid.NewGuid(),
                    Name = "Test book 2",
                    Type = BookType.Science,
                    PublishDate = new DateTime(2014, 02, 11),
                    Price = 15
                }
            );
        }
    }
}