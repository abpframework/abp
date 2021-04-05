using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace Volo.Abp.TextTemplating.Razor
{
    public interface IRazorProjectEngineFactory
    {
        Task<RazorProjectEngine> CreateAsync(Action<RazorProjectEngineBuilder> configure = null);
    }
}
