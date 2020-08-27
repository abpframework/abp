using System;
using JetBrains.Annotations;

namespace Volo.Abp.Users
{
    public interface ICurrentImpersonatorUser
    {
        [CanBeNull]
        Guid? Id { get; }
    }
}
