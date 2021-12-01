using System;
using Shouldly;
using Xunit;

namespace Volo.Abp.Auditing;

public class AuditPropertySetter_CreationAudit_Tests : AuditPropertySetterTestBase
{
    [Fact]
    public void Should_Do_Nothing_For_Non_Audited_Entity()
    {
        AuditPropertySetter.SetCreationProperties(new MyEmptyObject());
    }

    [Fact]
    public void Should_Set_CreationTime()
    {
        AuditPropertySetter.SetCreationProperties(TargetObject);

        TargetObject.CreationTime.ShouldBe(Now);
    }

    [Fact]
    public void Should_Not_Set_CreatorId_If_Current_User_Is_Not_Available()
    {
        AuditPropertySetter.SetCreationProperties(TargetObject);

        TargetObject.CreationTime.ShouldBe(Now);
        TargetObject.CreatorId.ShouldBe(null);
    }

    [Fact]
    public void Should_Set_CreatorId()
    {
        CurrentUserId = Guid.NewGuid();

        AuditPropertySetter.SetCreationProperties(TargetObject);

        TargetObject.CreationTime.ShouldBe(Now);
        TargetObject.CreatorId.ShouldBe(CurrentUserId);
    }

    [Fact]
    public void Should_Not_Set_CreatorId_If_It_Is_Already_Set()
    {
        var oldCreatorUserId = Guid.NewGuid();

        CurrentUserId = Guid.NewGuid();
        TargetObject.CreatorId = oldCreatorUserId;

        AuditPropertySetter.SetCreationProperties(TargetObject);

        TargetObject.CreationTime.ShouldBe(Now);
        TargetObject.CreatorId.ShouldBe(oldCreatorUserId);
    }

    [Fact]
    public void Should_Set_CreatorId_If_Entity_Tenant_Is_Same_With_Current_User_Tenant()
    {
        CurrentTenantId = Guid.NewGuid();
        CurrentUserId = Guid.NewGuid();

        CurrentUserTenantId = CurrentTenantId;
        TargetObject.TenantId = CurrentTenantId;

        AuditPropertySetter.SetCreationProperties(TargetObject);

        TargetObject.CreationTime.ShouldBe(Now);
        TargetObject.CreatorId.ShouldBe(CurrentUserId);
    }

    [Fact]
    public void Should_Not_Set_CreatorId_If_Entity_Tenant_Is_Different_From_Current_User_Tenant()
    {
        CurrentTenantId = Guid.NewGuid();
        CurrentUserId = Guid.NewGuid();

        CurrentUserTenantId = CurrentTenantId;
        TargetObject.TenantId = Guid.NewGuid();

        AuditPropertySetter.SetCreationProperties(TargetObject);

        TargetObject.CreationTime.ShouldBe(Now);
        TargetObject.CreatorId.ShouldBe(null);
    }
}
