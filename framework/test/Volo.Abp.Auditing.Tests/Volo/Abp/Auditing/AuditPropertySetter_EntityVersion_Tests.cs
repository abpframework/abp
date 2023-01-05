using Shouldly;
using Xunit;

namespace Volo.Abp.Auditing;

public class AuditPropertySetter_EntityVersion_Tests : AuditPropertySetterTestBase
{
    [Fact]
    public void Should_Do_Nothing_For_Non_Audited_Entity()
    {
        AuditPropertySetter.IncrementEntityVersionProperty(new MyEmptyObject());
    }
    
    [Fact]
    public void Should_Increment_EntityVersion()
    {
        AuditPropertySetter.IncrementEntityVersionProperty(TargetObject);

        TargetObject.EntityVersion.ShouldBe(1);
    }
}
