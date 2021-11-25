using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Options;

namespace Microsoft.Extensions.Options;

public static class OptionsAbpDynamicOptionsManagerExtensions
{
    public static Task SetAsync<T>(this IOptions<T> options)
        where T : class
    {
        return options.ToDynamicOptions().SetAsync();
    }

    public static Task SetAsync<T>(this IOptions<T> options, string name)
        where T : class
    {
        return options.ToDynamicOptions().SetAsync(name);
    }

    private static AbpDynamicOptionsManager<T> ToDynamicOptions<T>(this IOptions<T> options)
        where T : class
    {
        if (options is AbpDynamicOptionsManager<T> dynamicOptionsManager)
        {
            return dynamicOptionsManager;
        }

        throw new AbpException($"Options must be derived from the {typeof(AbpDynamicOptionsManager<>).FullName}!");
    }
}
