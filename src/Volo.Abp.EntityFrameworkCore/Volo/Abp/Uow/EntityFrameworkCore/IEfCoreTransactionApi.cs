using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    public interface IEfCoreTransactionApi : ITransactionApi
    {
        IDbContextTransaction DbContextTransaction { get; }

        DbContext StarterDbContext { get; }

        List<DbContext> AttendedDbContexts { get; }
    }
}