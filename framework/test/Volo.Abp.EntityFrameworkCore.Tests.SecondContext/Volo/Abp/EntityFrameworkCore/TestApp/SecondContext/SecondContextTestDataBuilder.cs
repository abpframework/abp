using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    public class SecondContextTestDataBuilder : ITransientDependency
    {
        private readonly IBasicRepository<BookInSecondDbContext, Guid> _bookRepository;
        private readonly IGuidGenerator _guidGenerator;

        public SecondContextTestDataBuilder(IBasicRepository<BookInSecondDbContext, Guid> bookRepository, IGuidGenerator guidGenerator)
        {
            _bookRepository = bookRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task BuildAsync()
        {
            await _bookRepository.InsertAsync(
                new BookInSecondDbContext(
                    _guidGenerator.Create(),
                    "TestBook1"
                )
            );
        }
    }
}