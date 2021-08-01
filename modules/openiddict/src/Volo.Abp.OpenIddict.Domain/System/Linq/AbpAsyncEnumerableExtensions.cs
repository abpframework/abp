using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Linq;

namespace System.Linq
{
    public static class AbpAsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(
            this IAsyncEnumerable<T> items,
            CancellationToken cancellationToken = default)
        {
            var results = new List<T>();
            await foreach (var item in items.WithCancellation(cancellationToken)
                                            .ConfigureAwait(false))
                results.Add(item);
            return results;
        }

        public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (var item in source)
            {
                yield return await Task.FromResult(item);
            }
        }
    }
}
