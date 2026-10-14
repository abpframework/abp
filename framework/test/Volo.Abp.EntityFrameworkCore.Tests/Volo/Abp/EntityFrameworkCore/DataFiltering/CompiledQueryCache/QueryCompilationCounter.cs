using System.Threading;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class QueryCompilationCounter
{
    private int _count;
    public int Count => Volatile.Read(ref _count);

    public void Increment()
    {
        Interlocked.Increment(ref _count);
    }
}
