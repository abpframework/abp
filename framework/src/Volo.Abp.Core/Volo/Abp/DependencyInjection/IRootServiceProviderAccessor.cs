using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

/// <summary>
/// The root service provider of the application.
/// Be careful to use the root service provider since there is no way
/// to release/dispose objects resolved from the root service provider.
/// So, always create a new scope if you need to resolve any service.
/// </summary>
public interface IRootServiceProvider : IKeyedServiceProvider
{

}
