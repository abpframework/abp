using System.Data;

namespace Volo.Abp.Domain.Repositories.Dapper
{
    public interface IDapperRepository
    {
        IDbConnection DbConnection { get; }

        IDbTransaction DbTransaction { get; }
    }
}