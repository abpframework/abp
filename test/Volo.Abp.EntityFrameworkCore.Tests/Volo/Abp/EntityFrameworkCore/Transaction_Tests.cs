using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore
{
    public class Transaction_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public Transaction_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IRepository<Person>>();
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Rollback_Transaction_When_An_Exception_Is_Thrown()
        {
            var personId = Guid.NewGuid();
            const string exceptionMessage = "thrown to rollback the transaction!";

            try
            {
                await WithUnitOfWorkAsync(async () =>
                {
                    await _personRepository.InsertAsync(new Person(personId, "Adam", 42));
                    throw new Exception(exceptionMessage);
                });
            }
            catch (Exception e) when (e.Message == exceptionMessage)
            {

            }

            var person = await _personRepository.FindAsync(personId);
            person.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Rollback_Transaction_Manually()
        {
            var personId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();
                await _personRepository.InsertAsync(new Person(personId, "Adam", 42));
                await _unitOfWorkManager.Current.RollbackAsync();
            });

            var person = await _personRepository.FindAsync(personId);
            person.ShouldBeNull();
        }
    }
}
