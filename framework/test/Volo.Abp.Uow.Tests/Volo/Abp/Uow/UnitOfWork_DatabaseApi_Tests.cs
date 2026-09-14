using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Uow;

public class UnitOfWork_DatabaseApi_Tests : AbpIntegratedTest<AbpUnitOfWorkModule>
{
    private const string KeyWithSecret = "MyDbContext_Server=localhost;User Id=sa;Password=SecretMarker;";

    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWork_DatabaseApi_Tests()
    {
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public void Should_Not_Expose_Database_Api_Key_On_Duplicate()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.AddDatabaseApi(KeyWithSecret, new FakeDatabaseApi());

            var exception = Assert.Throws<AbpException>(() => uow.AddDatabaseApi(KeyWithSecret, new FakeDatabaseApi()));
            exception.ToString().ShouldNotContain("SecretMarker");
        }
    }

    [Fact]
    public void Should_Not_Expose_Transaction_Api_Key_On_Duplicate()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.AddTransactionApi(KeyWithSecret, new FakeTransactionApi());

            var exception = Assert.Throws<AbpException>(() => uow.AddTransactionApi(KeyWithSecret, new FakeTransactionApi()));
            exception.ToString().ShouldNotContain("SecretMarker");
        }
    }

    private class FakeDatabaseApi : IDatabaseApi
    {

    }

    private class FakeTransactionApi : ITransactionApi
    {
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
