using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization
{
    public class DefaultLanguageProvider : ILanguageProvider, ITransientDependency
    {
        protected AbpLocalizationOptions Options { get; }

        public DefaultLanguageProvider(IOptions<AbpLocalizationOptions> options)
        {
            Options = options.Value;
        }

        public Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
        {
            return Task.FromResult((IReadOnlyList<LanguageInfo>)Options.Languages);
        }
    }
}