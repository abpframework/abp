using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.OpenIddict;

public static class AsyncEnumerableExtensions
{
    public static Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return ExecuteAsync();

        async Task<List<T>> ExecuteAsync()
        {
            var list = new List<T>();

            await foreach (var element in source)
            {
                list.Add(element);
            }

            return list;
        }
    }
}
