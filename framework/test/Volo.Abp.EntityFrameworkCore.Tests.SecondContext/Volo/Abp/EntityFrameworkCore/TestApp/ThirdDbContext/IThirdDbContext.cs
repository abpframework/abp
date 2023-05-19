using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;

public interface IThirdDbContext : IEfCoreDbContext
{
    DbSet<ThirdDbContextDummyEntity> DummyEntities { get; set; }
}
