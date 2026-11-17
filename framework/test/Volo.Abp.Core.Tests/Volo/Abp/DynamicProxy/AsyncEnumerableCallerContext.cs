using System.Threading;

namespace Volo.Abp.DynamicProxy;

public static class AsyncEnumerableCallerContext
{
    public static readonly AsyncLocal<string?> Value = new();
}
