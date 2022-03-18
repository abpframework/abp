using System.Threading.Tasks;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MemoryDb.DomainEvents;

public class DomainEvents_Tests : DomainEvents_Tests<AbpMemoryDbTestModule>
{
    [Fact(Skip = "MemoryDB doesn't support transactions.")]
    public override Task Should_Rollback_Uow_If_Event_Handler_Throws_Exception()
    {
        return base.Should_Rollback_Uow_If_Event_Handler_Throws_Exception();
    }
}
