using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Application.Services.QueryProjection;

public interface IBookNameSuffixProvider
{
    Task<string> GetAsync();
}

public class BookNameSuffixProvider : IBookNameSuffixProvider, ITransientDependency
{
    public async Task<string> GetAsync()
    {
        await Task.Yield();

        return BookAsyncProjectionAppService.Marker;
    }
}
