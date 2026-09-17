using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Bundling;

public interface IMauiBlazorContentFileProvider : IFileProvider
{
    string ContentRootPath { get; }
}