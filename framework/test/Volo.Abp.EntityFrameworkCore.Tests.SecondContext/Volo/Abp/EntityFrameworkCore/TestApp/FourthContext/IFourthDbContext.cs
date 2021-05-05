using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.TestApp.FourthContext
{
    public interface IFourthDbContext : IEfCoreDbContext
    {
        DbSet<FourthDbContextDummyEntity> FourthDummyEntities { get; set; }
    }
}
