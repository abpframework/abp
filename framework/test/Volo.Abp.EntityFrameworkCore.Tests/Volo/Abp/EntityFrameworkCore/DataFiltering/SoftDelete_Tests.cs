using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering
{
    /// <summary>
    /// This is just to test the cascade delete behavior of EF Core's navigation properties.
    /// Soft delete is usually only used in the aggregate root entity instead <see cref="Book" />.
    /// </summary>
    public class SoftDelete_Tests : SoftDelete_Tests<AbpEntityFrameworkCoreTestModule>
    {
        [Fact]
        public async Task Navigation_Properties_Cascade_Delete_Test()
        {
            var authorRepository = GetRequiredService<IRepository<Author, Guid>>();
            var authorId = Guid.NewGuid();
            
            var author = new Author(authorId, "tom");
            author.Books.Add(new Book(authorId,  Guid.NewGuid(), "asp net core"));
            author.Books.Add(new Book(authorId, Guid.NewGuid(),"c#"));
            await authorRepository.InsertAsync(author);
            
            await WithUnitOfWorkAsync(async () =>
            {
                var author = await authorRepository.GetAsync(authorId);
                author.Books.ShouldNotBeEmpty();
                author.Books.Count.ShouldBe(2);
                
                author.Books.Clear();
                await authorRepository.UpdateAsync(author);
            });
            
            using (DataFilter.Disable<ISoftDelete>())
            {
                author = await authorRepository.GetAsync(authorId);
                author.Books.ShouldNotBeEmpty();
                author.Books.ShouldAllBe(x => x.IsDeleted);
            }
        }
    }
}
