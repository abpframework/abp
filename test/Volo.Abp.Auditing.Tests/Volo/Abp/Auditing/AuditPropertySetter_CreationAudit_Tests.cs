using System;
using Shouldly;
using Xunit;

namespace Volo.Abp.Auditing
{
    public class AuditPropertySetter_CreationAudit_Tests : AuditPropertySetterTestBase
    {
        [Fact]
        public void Should_Set_CreationTime()
        {
            AuditPropertySetter.SetCreationAuditProperties(TargetObject);

            TargetObject.CreationTime.ShouldNotBe(default);
        }

        [Fact]
        public void Should_Set_CreatorId()
        {
            CurrentUserId = Guid.NewGuid();

            AuditPropertySetter.SetCreationAuditProperties(TargetObject);

            TargetObject.CreationTime.ShouldNotBe(default);
            TargetObject.CreatorId.ShouldBe(CurrentUserId);
        }
    }
}