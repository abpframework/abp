using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity
{
    public class IdentityTestData : ISingletonDependency
    {
        public Guid RoleModeratorId { get; } = Guid.NewGuid();

        public Guid UserJohnId { get; } = Guid.NewGuid();
        public Guid UserDavidId { get; } = Guid.NewGuid();
        public Guid UserNeoId { get; } = Guid.NewGuid();
        public Guid UserBobId { get; } = Guid.NewGuid();
        public Guid AgeClaimId { get; } = Guid.NewGuid();
        public Guid EducationClaimId { get; } = Guid.NewGuid();
    }
}
