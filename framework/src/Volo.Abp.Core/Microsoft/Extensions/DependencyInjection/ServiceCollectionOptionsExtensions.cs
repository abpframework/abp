using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionOptionsExtensions
{
    /// <summary>
    /// You should only use this method to register options if you need to continue using the ServiceProvider to get other options in your Options configuration method.
    /// Otherwise, please use the default AddOptions method for better performance.
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public static OptionsBuilder<TOptions> AddAbpOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        services.TryAddSingleton<IOptions<TOptions>, AbpUnnamedOptionsManager<TOptions>>();
        return services.AddOptions<TOptions>();
    }
}
