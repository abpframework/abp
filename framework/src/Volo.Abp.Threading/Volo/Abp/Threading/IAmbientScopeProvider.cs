using System;

namespace Volo.Abp.Threading;

public interface IAmbientScopeProvider<T>
{
    T GetValue(string contextKey);

    IDisposable BeginScope(string contextKey, T value);
}
