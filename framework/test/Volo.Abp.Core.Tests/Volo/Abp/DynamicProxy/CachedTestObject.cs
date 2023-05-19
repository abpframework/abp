using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class CachedTestObject
{
    public virtual int GetValue(int v)
    {
        Thread.Sleep(5);
        return v;
    }

    public virtual async Task<int> GetValueAsync(int v)
    {
        await Task.Delay(5);
        return v;
    }
}
