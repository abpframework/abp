using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    public class SecondContextTestDataBuilder : ITransientDependency
    {
        private readonly IRepository<BookInSecondDbContext> _bookRepository;
        private readonly IGuidGenerator _guidGenerator;

        public SecondContextTestDataBuilder(IRepository<BookInSecondDbContext> bookRepository, IGuidGenerator guidGenerator)
        {
            _bookRepository = bookRepository;
            _guidGenerator = guidGenerator;
        }

        public void Build()
        {
            _bookRepository.Insert(new BookInSecondDbContext
            {
                Id = _guidGenerator.Create(),
                Name = "TestBook1"
            });
        }
    }
}