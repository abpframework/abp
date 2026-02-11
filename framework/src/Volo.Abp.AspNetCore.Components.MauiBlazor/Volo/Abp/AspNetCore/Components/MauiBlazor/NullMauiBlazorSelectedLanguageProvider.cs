using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

public class NullMauiBlazorSelectedLanguageProvider : IMauiBlazorSelectedLanguageProvider, ITransientDependency
{
    public Task<string?> GetSelectedLanguageAsync()
    {
        return Task.FromResult((string?)null);
    }
}