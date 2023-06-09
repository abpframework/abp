using System;
using JetBrains.Annotations;

namespace Volo.Abp.Tracing;

public interface ICorrelationIdProvider
{
    [NotNull]
    string Get();

    IDisposable Change(string correlationId);
}
