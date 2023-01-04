using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public class EmptyWebAssemblyHostEnvironment : IWebAssemblyHostEnvironment
{
    public string Environment { get; set; }

    public string BaseAddress { get; set; }
}
