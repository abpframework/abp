using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application;

public class StreamingAppService : ApplicationService
{
    public virtual async IAsyncEnumerable<int> GetItemsAsync()
    {
        await Task.Yield();
        yield return 1;
        yield return 2;
    }

    public virtual async IAsyncEnumerable<int> GetSlowItemsAsync()
    {
        await Task.Delay(50);
        yield return 1;
    }

    public virtual IAsyncEnumerable<int> GetItemsWithCreationErrorAsync()
    {
        throw new UserFriendlyException("Failed to create the sequence");
    }
}
