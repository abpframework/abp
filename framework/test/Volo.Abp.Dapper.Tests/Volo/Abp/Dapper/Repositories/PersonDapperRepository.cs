using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TestApp.EntityFrameworkCore;

namespace Volo.Abp.Dapper.Repositories
{
    public class PersonDapperRepository : DapperRepository<TestAppDbContext>, ITransientDependency
    {
        public PersonDapperRepository(IDbContextProvider<TestAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<List<string>> GetAllPersonNames()
        {
            return (await (await GetDbConnectionAsync())
                    .QueryAsync<string>(
                        "select Name from People",
                        transaction: await GetDbTransactionAsync()
                    )
                ).ToList();
        }

        public virtual async Task<int> UpdatePersonNames(string name)
        {
            return await (await GetDbConnectionAsync())
                .ExecuteAsync("update People set Name = @NewName", new {NewName = name},
                    await GetDbTransactionAsync());
        }
    }
}
