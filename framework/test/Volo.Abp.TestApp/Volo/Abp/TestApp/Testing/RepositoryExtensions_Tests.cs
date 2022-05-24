using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing;

public abstract class RepositoryExtensions_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly IRepository<Person, Guid> PersonRepository;

    protected RepositoryExtensions_Tests()
    {
        PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
    }

    [Fact]
    public async Task EnsureExistsAsync_Test()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var id = Guid.NewGuid();
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await PersonRepository.EnsureExistsAsync(Guid.NewGuid())
            );
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await PersonRepository.EnsureExistsAsync(x => x.Id == id)
            );

            await PersonRepository.EnsureExistsAsync(TestDataBuilder.UserDouglasId);
            await PersonRepository.EnsureExistsAsync(x => x.Id == TestDataBuilder.UserDouglasId);
        });
    }
}
