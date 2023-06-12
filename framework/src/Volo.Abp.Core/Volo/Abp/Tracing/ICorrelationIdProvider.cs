using System;

namespace Volo.Abp.Tracing;

public interface ICorrelationIdProvider
{
    string? Get();

    IDisposable Change(string? correlationId);
}
