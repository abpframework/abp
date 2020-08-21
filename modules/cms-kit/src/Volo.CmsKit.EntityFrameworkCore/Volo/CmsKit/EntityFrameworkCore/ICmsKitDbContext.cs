using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.CmsKit.EntityFrameworkCore
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public interface ICmsKitDbContext : IEfCoreDbContext
    {

    }
}
