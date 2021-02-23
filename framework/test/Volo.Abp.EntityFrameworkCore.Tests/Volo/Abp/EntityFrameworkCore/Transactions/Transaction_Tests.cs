using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Transactions
{
    public class Transaction_Tests : TestAppTestBase<AbpEntityFrameworkCoreTestModule>
    {
        private readonly IBasicRepository<Person, Guid> _personRepository;
        private readonly IBasicRepository<BookInSecondDbContext, Guid> _bookRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public Transaction_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IBasicRepository<Person, Guid>>();
            _bookRepository = ServiceProvider.GetRequiredService<IBasicRepository<BookInSecondDbContext, Guid>>();
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Rollback_Transaction_When_An_Exception_Is_Thrown()
        {
            var personId = Guid.NewGuid();
            const string exceptionMessage = "thrown to rollback the transaction!";

            try
            {
                await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions { IsTransactional = true }, async () =>
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

            await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions { IsTransactional = true }, async () =>
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();

                await _personRepository.InsertAsync(new Person(personId, "Adam", 42));

                await _unitOfWorkManager.Current.RollbackAsync();
            });

            var person = await _personRepository.FindAsync(personId);
            person.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Rollback_Transaction_Manually_With_Double_DbContext_Transaction()
        {
            var personId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (uowManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
                {
                    _unitOfWorkManager.Current.ShouldNotBeNull();

                    await _personRepository.InsertAsync(new Person(personId, "Adam", 42));
                    await _bookRepository.InsertAsync(new BookInSecondDbContext(bookId, bookId.ToString()));

                    await _unitOfWorkManager.Current.SaveChangesAsync();

                    //Will automatically rollback since not called the Complete!
                }
            }

            (await _personRepository.FindAsync(personId)).ShouldBeNull();
            (await _bookRepository.FindAsync(bookId)).ShouldBeNull();
        }


        [Fact]
        public async Task Should_Rollback()
        {
            var options = new AbpUnitOfWorkOptions {IsTransactional = true};
            var personId = Guid.NewGuid();

            //AbpUnitOfWorkMiddleware
            //https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore/Volo/Abp/AspNetCore/Uow/AbpUnitOfWorkMiddleware.cs#L19-L23
            using (var middlewareUow = _unitOfWorkManager.Reserve("Reservation1"))
            {
                //If UnitOfWorkInterceptor is executed between AbpUnitOfWorkMiddleware and AbpUowActionFilter, uow will be used in advance.
                //UnitOfWorkInterceptor
                //https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.Uow/Volo/Abp/Uow/UnitOfWorkInterceptor.cs#L34-L45
                {
                    //Cancel this call unit test will success.
                    // if (_unitOfWorkManager.TryBeginReserved("Reservation1", options))
                    // {
                    //
                    // }
                }

                //_unitOfWorkManager.Current is middlewareUow

                //AbpUowActionFilter
                //https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore.Mvc/Volo/Abp/AspNetCore/Mvc/Uow/AbpUowActionFilter.cs#L42-L61
                {
                    if (_unitOfWorkManager.TryBeginReserved("Reservation1", options))
                    {
                        try
                        {
                            await _personRepository.InsertAsync(new Person(personId, "Adam", 42));
                            throw new NotImplementedException();
                        }
                        catch (Exception e)
                        {
                            await _unitOfWorkManager.Current.RollbackAsync();
                        }
                    }

                    using (var filterUow = _unitOfWorkManager.Begin(options))
                    {
                        var success = false;
                        try
                        {
                            await _personRepository.InsertAsync(new Person(personId, "Adam", 42));
                            throw new NotImplementedException();
                            success = true;
                        }
                        catch (Exception e)
                        {
                            success = false;
                            //Exception will be handled by AbpExceptionFilter
                        }

                        if (success)
                        {
                            await filterUow.CompleteAsync();
                        }
                    }
                }

                await middlewareUow.CompleteAsync();

                (await _personRepository.FindAsync(personId)).ShouldBeNull();
            }
        }
    }
}
