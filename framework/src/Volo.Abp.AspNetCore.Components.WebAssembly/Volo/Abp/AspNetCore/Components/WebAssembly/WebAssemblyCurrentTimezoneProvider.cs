using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

[Dependency(ReplaceServices = true)]
public class WebAssemblyCurrentTimezoneProvider : ICurrentTimezoneProvider, ISingletonDependency
{
    public string? TimeZone { get; set; }
}
