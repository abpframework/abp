using System;
using Shouldly;
using Xunit;

namespace Volo.Abp.Auditing;

public class AuditPropertySetter_ModificationAudit_Tests : AuditPropertySetterTestBase
{
    [Fact]
    public void Should_Do_Nothing_For_Non_Audited_Entity()
    {
        AuditPropertySetter.SetModificationProperties(new MyEmptyObject());
    }

    [Fact]
    public void Should_Set_LastModificationTime()
    {
        AuditPropertySetter.SetModificationProperties(TargetObject);

        TargetObject.LastModificationTime.ShouldBe(Now);
    }

    [Fact]
    public void Should_Clear_LastModifierId_If_Current_User_Is_Not_Available()
    {
        TargetObject.LastModifierId = Guid.NewGuid();

        AuditPropertySetter.SetModificationProperties(TargetObject);

        TargetObject.LastModificationTime.ShouldBe(Now);
        TargetObject.LastModifierId.ShouldBe(null);
    }

    [Fact]
    public void Should_Set_LastModifierId()
    {
        CurrentUserId = Guid.NewGuid();

        AuditPropertySetter.SetModificationProperties(TargetObject);

        TargetObject.LastModificationTime.ShouldBe(Now);
        TargetObject.LastModifierId.ShouldBe(CurrentUserId);
    }

    [Fact]
    public void Should_Set_LastModifierId_Again_Even_If_It_Is_Set_Before()
    {
        CurrentUserId = Guid.NewGuid();
        TargetObject.LastModifierId = Guid.NewGuid();

        AuditPropertySetter.SetModificationProperties(TargetObject);

        TargetObject.LastModificationTime.ShouldBe(Now);
        TargetObject.LastModifierId.ShouldBe(CurrentUserId);
    }

    [Fact]
    public void Should_Set_LastModifierId_If_Entity_Tenant_Is_Same_With_Current_User_Tenant()
    {
        CurrentTenantId = Guid.NewGuid();
        CurrentUserId = Guid.NewGuid();

        CurrentUserTenantId = CurrentTenantId;
        TargetObject.TenantId = CurrentTenantId;

        AuditPropertySetter.SetModificationProperties(TargetObject);

        TargetObject.LastModificationTime.ShouldBe(Now);
        TargetObject.LastModifierId.ShouldBe(CurrentUserId);
    }

    [Fact]
    public void Should_Clear_LastModifierId_If_Entity_Tenant_Is_Different_From_Current_User_Tenant()
    {
        CurrentTenantId = Guid.NewGuid();
        CurrentUserId = Guid.NewGuid();
        CurrentUserTenantId = CurrentTenantId;
        TargetObject.TenantId = Guid.NewGuid();
        TargetObject.LastModifierId = Guid.NewGuid();

        AuditPropertySetter.SetModificationProperties(TargetObject);

        TargetObject.LastModificationTime.ShouldBe(Now);
        TargetObject.LastModifierId.ShouldBe(null);
    }
}
