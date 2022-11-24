using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

public interface IMauiBlazorSelectedLanguageProvider
{
    Task<string> GetSelectedLanguageAsync();
}