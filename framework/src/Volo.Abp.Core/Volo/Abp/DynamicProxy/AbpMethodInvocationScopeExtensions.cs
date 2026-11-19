using System;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public static class AbpMethodInvocationScopeExtensions
{
    /// <summary>
    /// Also runs each enumeration of a returned <see cref="System.Collections.Generic.IAsyncEnumerable{T}"/> in a new scope.
    /// <paramref name="beginScope"/> must set its async local values (e.g. the ambient unit of work) synchronously.
    /// </summary>
    public static async Task ProceedInScopeAsync(this IAbpMethodInvocation invocation, Func<IAbpInterceptionScope> beginScope)
    {
        using (var scope = beginScope())
        {
            try
            {
                await invocation.ProceedAsync();
            }
            catch (Exception exception)
            {
                await scope.FailAsync(exception);
                throw;
            }

            await scope.CompleteAsync(default);
        }

        invocation.WrapAsyncEnumerable(beginScope);
    }
}
