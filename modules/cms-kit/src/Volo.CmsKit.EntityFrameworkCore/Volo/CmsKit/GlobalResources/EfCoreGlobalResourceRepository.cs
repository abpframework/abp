using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.GlobalResources;

public class EfCoreGlobalResourceRepository: EfCoreRepository<ICmsKitDbContext, GlobalResource, Guid>, IGlobalResourceRepository
{
    public EfCoreGlobalResourceRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
        
    }

    public Task<GlobalResource> FindByName(string name,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(name, nameof(name));
        return FindAsync(x => x.Name == name, cancellationToken: GetCancellationToken(cancellationToken));
    }
}