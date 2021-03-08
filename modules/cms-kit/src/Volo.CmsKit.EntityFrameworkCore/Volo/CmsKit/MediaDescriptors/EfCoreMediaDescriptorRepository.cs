using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.MediaDescriptors
{
    public class EfCoreMediaDescriptorRepository : EfCoreRepository<ICmsKitDbContext, MediaDescriptor, Guid>, IMediaDescriptorRepository
    {
        public EfCoreMediaDescriptorRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}