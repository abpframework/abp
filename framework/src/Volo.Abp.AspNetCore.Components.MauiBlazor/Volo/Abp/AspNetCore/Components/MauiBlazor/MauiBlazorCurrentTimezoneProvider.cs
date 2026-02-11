using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

[Dependency(ReplaceServices = true)]
public class MauiBlazorCurrentTimezoneProvider : ICurrentTimezoneProvider, ISingletonDependency
{
    public string? TimeZone { get; set; }
}
