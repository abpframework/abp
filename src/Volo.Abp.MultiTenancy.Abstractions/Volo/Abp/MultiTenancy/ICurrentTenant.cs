using System;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.MultiTenancy
{
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        [CanBeNull]
        Guid? Id { get; }

        [CanBeNull]
        string Name { get; }

        [CanBeNull]
        ConnectionStrings ConnectionStrings { get; }

        [NotNull]
        IDisposable Change(Guid? id);

        [NotNull]
        IDisposable Change([CanBeNull] string name);
    }
}
