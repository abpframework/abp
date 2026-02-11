using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.DependencyInjection;

internal class EmptyHostingEnvironment : IWebHostEnvironment
{
    public string EnvironmentName { get; set; } = default!;

    public string ApplicationName { get; set; } = default!;

    public string WebRootPath { get; set; } = default!;

    public IFileProvider WebRootFileProvider { get; set; } = default!;

    public string ContentRootPath { get; set; } = default!;

    public IFileProvider ContentRootFileProvider { get; set; } = default!;
}