using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace Volo.Abp.TextTemplating.Razor
{
    public interface IAbpRazorProjectEngineFactory
    {
        Task<RazorProjectEngine> CreateAsync(Action<RazorProjectEngineBuilder> configure = null);
    }
}
